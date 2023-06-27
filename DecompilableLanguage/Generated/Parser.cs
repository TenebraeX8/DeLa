using DecompilableLanguage.Instructions;
using DecompilableLanguage.Compiler.Symbols;
using DecompilableLanguage.Compiler.Code;



using System;



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _number = 2;
	public const int _charCon = 3;
	public const int maxT = 19;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public SymbolTable tab {get;set;}
public CodeGenerator code {get;set;}

public void Error(string text)
{
    Console.WriteLine(text);
    this.errors.count++;
}



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void DeLa() {
		Expect(4);
		Expect(1);
		tab.ScriptName(t.val);
		VarDecl();
		Statements();
		Expect(5);
		Expect(1);
		Expect(6);
	}

	void VarDecl() {
		Expect(7);
		Expect(1);
		tab.Insert(t.val, SymbolObj.Var, SymbolTable.IntegerType);
		while (la.kind == 8) {
			Get();
			Expect(1);
			tab.Insert(t.val, SymbolObj.Var, SymbolTable.IntegerType);
		}
		Expect(9);
	}

	void Statements() {
		Statement();
		while (la.kind == 9) {
			Get();
			Statement();
		}
	}

	void Statement() {
		if (la.kind == 1 || la.kind == 11) {
			if (la.kind == 1) {
				Get();
				var obj = tab.Find(t.val); 
				Expect(10);
				Expression();
				if(obj != null)
				   code.Store(obj.adr);
				
			} else {
				Get();
				Expression();
			}
		}
	}

	void Expression() {
		bool isNeg = false; byte op;
		if (la.kind == 14 || la.kind == 15) {
			Addop(out op);
			if(op == Instruction.SUB) isNeg = true;
		}
		Term();
		while (la.kind == 14 || la.kind == 15) {
			Addop(out op);
			Term();
			code.Instr(op);
		}
	}

	void Addop(out byte op) {
		op = 0;
		if (la.kind == 14) {
			Get();
			op = Instruction.ADD;
		} else if (la.kind == 15) {
			Get();
			op = Instruction.SUB;
		} else SynErr(20);
	}

	void Term() {
		byte op;
		Factor();
		while (la.kind == 16 || la.kind == 17 || la.kind == 18) {
			Mulop(out op);
			Factor();
			code.Instr(op);
		}
	}

	void Factor() {
		if (la.kind == 1) {
			Get();
			var obj = tab.Find(t.val);
			if(obj != null)
			   code.Load(obj.adr);
			
		} else if (la.kind == 2) {
			Get();
			code.Push(int.Parse(t.val));
		} else if (la.kind == 12) {
			Get();
			Expression();
			Expect(13);
		} else SynErr(21);
	}

	void Mulop(out byte op) {
		op = 0;
		if (la.kind == 16) {
			Get();
			op = Instruction.MUL;
		} else if (la.kind == 17) {
			Get();
			op = Instruction.DIV;
		} else if (la.kind == 18) {
			Get();
			op = Instruction.MOD;
		} else SynErr(22);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		DeLa();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "charCon expected"; break;
			case 4: s = "\"SCRIPT\" expected"; break;
			case 5: s = "\"END\" expected"; break;
			case 6: s = "\".\" expected"; break;
			case 7: s = "\"VAR\" expected"; break;
			case 8: s = "\",\" expected"; break;
			case 9: s = "\";\" expected"; break;
			case 10: s = "\"=\" expected"; break;
			case 11: s = "\"OUT\" expected"; break;
			case 12: s = "\"(\" expected"; break;
			case 13: s = "\")\" expected"; break;
			case 14: s = "\"+\" expected"; break;
			case 15: s = "\"-\" expected"; break;
			case 16: s = "\"*\" expected"; break;
			case 17: s = "\"/\" expected"; break;
			case 18: s = "\"%\" expected"; break;
			case 19: s = "??? expected"; break;
			case 20: s = "invalid Addop"; break;
			case 21: s = "invalid Factor"; break;
			case 22: s = "invalid Mulop"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}

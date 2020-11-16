using Library;



using System;
using System.IO;
using System.Text;

namespace Assem {

public class Parser {
	public const int _EOF = 0;
	public const int _identifier = 1;
	public const int _number = 2;
	public const int _label = 3;
	public const int _stringLit = 4;
	public const int _Comment = 5;
	public const int _EOL = 6;
	// terminals
	public const int EOF_SYM = 0;
	public const int identifier_Sym = 1;
	public const int number_Sym = 2;
	public const int label_Sym = 3;
	public const int stringLit_Sym = 4;
	public const int Comment_Sym = 5;
	public const int EOL_Sym = 6;
	public const int assem_Sym = 7;
	public const int begin_Sym = 8;
	public const int end_Sym = 9;
	public const int point_Sym = 10;
	public const int add_Sym = 11;
	public const int and_Sym = 12;
	public const int anew_Sym = 13;
	public const int ceq_Sym = 14;
	public const int cge_Sym = 15;
	public const int cgt_Sym = 16;
	public const int cle_Sym = 17;
	public const int clt_Sym = 18;
	public const int cne_Sym = 19;
	public const int div_Sym = 20;
	public const int halt_Sym = 21;
	public const int inpb_Sym = 22;
	public const int inpi_Sym = 23;
	public const int ldv_Sym = 24;
	public const int ldxa_Sym = 25;
	public const int mul_Sym = 26;
	public const int neg_Sym = 27;
	public const int nop_Sym = 28;
	public const int not_Sym = 29;
	public const int or_Sym = 30;
	public const int prnb_Sym = 31;
	public const int prni_Sym = 32;
	public const int prnl_Sym = 33;
	public const int rem_Sym = 34;
	public const int sto_Sym = 35;
	public const int sub_Sym = 36;
	public const int ldaunderscore0_Sym = 37;
	public const int ldaunderscore1_Sym = 38;
	public const int ldcunderscore0_Sym = 39;
	public const int ldcunderscore1_Sym = 40;
	public const int inc_Sym = 41;
	public const int dec_Sym = 42;
	public const int dsp_Sym = 43;
	public const int ldc_Sym = 44;
	public const int lda_Sym = 45;
	public const int ldl_Sym = 46;
	public const int stl_Sym = 47;
	public const int prns_Sym = 48;
	public const int brn_Sym = 49;
	public const int bze_Sym = 50;
	public const int NOT_SYM = 51;
	// pragmas

	public const int maxT = 51;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;

	public static Token token;    // last recognized token   /* pdt */
	public static Token la;       // lookahead token
	static int errDist = minErrDist;

	const bool known = true;

  public static OutFile pretty;




	static void SynErr (int n) {
		if (errDist >= minErrDist) Errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public static void SemErr (string msg) {
		if (errDist >= minErrDist) Errors.Error(token.line, token.col, msg); /* pdt */
		errDist = 0;
	}

	public static void SemError (string msg) {
		if (errDist >= minErrDist) Errors.Error(token.line, token.col, msg); /* pdt */
		errDist = 0;
	}

	public static void Warning (string msg) { /* pdt */
		if (errDist >= minErrDist) Errors.Warn(token.line, token.col, msg);
		errDist = 2; //++ 2009/11/04
	}

	public static bool Successful() { /* pdt */
		return Errors.count == 0;
	}

	public static string LexString() { /* pdt */
		return token.val;
	}

	public static string LookAheadString() { /* pdt */
		return la.val;
	}

	static void Get () {
		for (;;) {
			token = la; /* pdt */
			la = Scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = token; /* pdt */
		}
	}

	static void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}

	static bool StartOf (int s) {
		return set[s, la.kind];
	}

	static void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}

	static bool WeakSeparator (int n, int syFol, int repFol) {
		bool[] s = new bool[maxT+1];
		if (la.kind == n) { Get(); return true; }
		else if (StartOf(repFol)) return false;
		else {
			for (int i=0; i <= maxT; i++) {
				s[i] = set[syFol, i] || set[repFol, i] || set[0, i];
			}
			SynErr(n);
			while (!s[la.kind]) Get();
			return StartOf(syFol);
		}
	}

	static void Assem() {
		Expect(assem_Sym);
		Expect(begin_Sym);
		if (la.kind == Comment_Sym) {
			Get();
			Expect(EOL_Sym);
		}
		while (StartOf(1)) {
			Statement();
		}
		Expect(end_Sym);
		Expect(point_Sym);
		if (la.kind == Comment_Sym) {
			Get();
		}
		LabelTable.CheckLabels();
		LabelTable.ListReferences(pretty);
		VarTable.ListReferences(pretty);
		pretty.Close();
	}

	static void Statement() {
		if (StartOf(2)) {
			OneWord();
		} else if (StartOf(3)) {
			TwoWord();
		} else if (la.kind == prns_Sym) {
			WriteString();
		} else if (la.kind == label_Sym) {
			Label();
		} else if (la.kind == brn_Sym || la.kind == bze_Sym) {
			Branch();
		} else SynErr(52);
	}

	static void OneWord() {
		switch (la.kind) {
		case add_Sym: {
			Get();
			break;
		}
		case and_Sym: {
			Get();
			break;
		}
		case anew_Sym: {
			Get();
			break;
		}
		case ceq_Sym: {
			Get();
			break;
		}
		case cge_Sym: {
			Get();
			break;
		}
		case cgt_Sym: {
			Get();
			break;
		}
		case cle_Sym: {
			Get();
			break;
		}
		case clt_Sym: {
			Get();
			break;
		}
		case cne_Sym: {
			Get();
			break;
		}
		case div_Sym: {
			Get();
			break;
		}
		case halt_Sym: {
			Get();
			break;
		}
		case inpb_Sym: {
			Get();
			break;
		}
		case inpi_Sym: {
			Get();
			break;
		}
		case ldv_Sym: {
			Get();
			break;
		}
		case ldxa_Sym: {
			Get();
			break;
		}
		case mul_Sym: {
			Get();
			break;
		}
		case neg_Sym: {
			Get();
			break;
		}
		case nop_Sym: {
			Get();
			break;
		}
		case not_Sym: {
			Get();
			break;
		}
		case or_Sym: {
			Get();
			break;
		}
		case prnb_Sym: {
			Get();
			break;
		}
		case prni_Sym: {
			Get();
			break;
		}
		case prnl_Sym: {
			Get();
			break;
		}
		case rem_Sym: {
			Get();
			break;
		}
		case sto_Sym: {
			Get();
			break;
		}
		case sub_Sym: {
			Get();
			break;
		}
		case ldaunderscore0_Sym: {
			Get();
			break;
		}
		case ldaunderscore1_Sym: {
			Get();
			break;
		}
		case ldcunderscore0_Sym: {
			Get();
			break;
		}
		case ldcunderscore1_Sym: {
			Get();
			break;
		}
		case inc_Sym: {
			Get();
			break;
		}
		case dec_Sym: {
			Get();
			break;
		}
		default: SynErr(53); break;
		}
		CodeGen.OneWord(token.val);
	}

	static void TwoWord() {
		int value;
		if (la.kind == dsp_Sym) {
			Get();
		} else if (la.kind == ldc_Sym) {
			Get();
		} else if (la.kind == lda_Sym) {
			Get();
		} else if (la.kind == ldl_Sym) {
			Get();
		} else if (la.kind == stl_Sym) {
			Get();
		} else SynErr(54);
		string mnemonic = token.val;
		if (la.kind == number_Sym) {
			Number(out value);
			CodeGen.TwoWord(mnemonic, value);
		} else if (la.kind == identifier_Sym) {
			Variable(out value);
			CodeGen.TwoWord(mnemonic,value);
		} else SynErr(55);
	}

	static void WriteString() {
		string str;
		Expect(prns_Sym);
		StringConst(out str);
		CodeGen.WriteString(str);
	}

	static void Label() {
		Expect(label_Sym);
		string name = token.val.Substring(0, token.val.Length - 1).ToLower();
		int lineNumber = -token.line;
		LabelEntry entry = LabelTable.Find(name);
		if (entry == null)
		  LabelTable.Insert(new LabelEntry(name, new Label(known),lineNumber));
		else if (entry.label.IsDefined())
		  SemError("redefined label");
		else {
		  entry.label.Here();
		  entry.AddReference(lineNumber);
		}
	}

	static void Branch() {
		int target;
		string name;
		Label lab;
		if (la.kind == brn_Sym) {
			Get();
		} else if (la.kind == bze_Sym) {
			Get();
		} else SynErr(56);
		string mnemonic = token.val;
		if (la.kind == number_Sym) {
			Number(out target);
			CodeGen.TwoWord(mnemonic, target);
		} else if (la.kind == identifier_Sym) {
			Ident(out name);
			LabelEntry entry = LabelTable.Find(name);
			if (entry == null) {
			  lab = new Label(!known);
			  int lineNumber = token.line;
			  LabelTable.Insert(new LabelEntry(name, lab, lineNumber));
			}
			else {
			  lab = entry.label;
			  entry.AddReference(token.line);
			}
			CodeGen.Branch(mnemonic, lab);
		} else SynErr(57);
	}

	static void Number(out int value) {
		Expect(number_Sym);
		try {
		  value = Convert.ToInt32(token.val);
		} catch (Exception) {
		  value = 0;
		  SemError("number too large");
		}
	}

	static void Variable(out int value) {
		Expect(identifier_Sym);
		try{
		    string ident_name = token.val;
		    int lineNumber = token.line;
		    value = VarTable.FindOffset(ident_name, lineNumber);
		   } catch (Exception){
		    value = 0;
		    SemError("invalid identifier");
		   }
		
	}

	static void StringConst(out string str) {
		Expect(stringLit_Sym);
		str = token.val.Substring(1, token.val.Length - 2);
	}

	static void Ident(out string name) {
		Expect(identifier_Sym);
		name = token.val.ToLower();
	}



	public static void Parse() {
		la = new Token();
		la.val = "";
		Get();
		Assem();
		Expect(EOF_SYM);

	}

	static bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,T, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, x,x,x,x, x}

	};

} // end Parser

/* pdt - considerable extension from here on */

public class ErrorRec {
	public int line, col, num;
	public string str;
	public ErrorRec next;

	public ErrorRec(int l, int c, string s) {
		line = l; col = c; str = s; next = null;
	}

} // end ErrorRec

public class Errors {

	public static int count = 0;                                     // number of errors detected
	public static int warns = 0;                                     // number of warnings detected
	public static string errMsgFormat = "file {0} : ({1}, {2}) {3}"; // 0=file 1=line, 2=column, 3=text
	static string fileName = "";
	static string listName = "";
	static bool mergeErrors = false;
	static StreamWriter mergedList;

	static ErrorRec first = null, last;
	static bool eof = false;

	static string GetLine() {
		char ch, CR = '\r', LF = '\n';
		int l = 0;
		StringBuilder s = new StringBuilder();
		ch = (char) Buffer.Read();
		while (ch != Buffer.EOF && ch != CR && ch != LF) {
			s.Append(ch); l++; ch = (char) Buffer.Read();
		}
		eof = (l == 0 && ch == Buffer.EOF);
		if (ch == CR) {  // check for MS-DOS
			ch = (char) Buffer.Read();
			if (ch != LF && ch != Buffer.EOF) Buffer.Pos--;
		}
		return s.ToString();
	}

	static void Display (string s, ErrorRec e) {
		mergedList.Write("**** ");
		for (int c = 1; c < e.col; c++)
			if (s[c-1] == '\t') mergedList.Write("\t"); else mergedList.Write(" ");
		mergedList.WriteLine("^ " + e.str);
	}

	public static void Init (string fn, string dir, bool merge) {
		fileName = fn;
		listName = dir + "listing.txt";
		mergeErrors = merge;
		if (mergeErrors)
			try {
				mergedList = new StreamWriter(new FileStream(listName, FileMode.Create));
			} catch (IOException) {
				Errors.Exception("-- could not open " + listName);
			}
	}

	public static void Summarize () {
		if (mergeErrors) {
			mergedList.WriteLine();
			ErrorRec cur = first;
			Buffer.Pos = 0;
			int lnr = 1;
			string s = GetLine();
			while (!eof) {
				mergedList.WriteLine("{0,4} {1}", lnr, s);
				while (cur != null && cur.line == lnr) {
					Display(s, cur); cur = cur.next;
				}
				lnr++; s = GetLine();
			}
			if (cur != null) {
				mergedList.WriteLine("{0,4}", lnr);
				while (cur != null) {
					Display(s, cur); cur = cur.next;
				}
			}
			mergedList.WriteLine();
			mergedList.WriteLine(count + " errors detected");
			if (warns > 0) mergedList.WriteLine(warns + " warnings detected");
			mergedList.Close();
		}
		switch (count) {
			case 0 : Console.WriteLine("Parsed correctly"); break;
			case 1 : Console.WriteLine("1 error detected"); break;
			default: Console.WriteLine(count + " errors detected"); break;
		}
		if (warns > 0) Console.WriteLine(warns + " warnings detected");
		if ((count > 0 || warns > 0) && mergeErrors) Console.WriteLine("see " + listName);
	}

	public static void StoreError (int line, int col, string s) {
		if (mergeErrors) {
			ErrorRec latest = new ErrorRec(line, col, s);
			if (first == null) first = latest; else last.next = latest;
			last = latest;
		} else Console.WriteLine(errMsgFormat, fileName, line, col, s);
	}

	public static void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "identifier expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "label expected"; break;
			case 4: s = "stringLit expected"; break;
			case 5: s = "Comment expected"; break;
			case 6: s = "EOL expected"; break;
			case 7: s = "\"assem\" expected"; break;
			case 8: s = "\"begin\" expected"; break;
			case 9: s = "\"end\" expected"; break;
			case 10: s = "\".\" expected"; break;
			case 11: s = "\"add\" expected"; break;
			case 12: s = "\"and\" expected"; break;
			case 13: s = "\"anew\" expected"; break;
			case 14: s = "\"ceq\" expected"; break;
			case 15: s = "\"cge\" expected"; break;
			case 16: s = "\"cgt\" expected"; break;
			case 17: s = "\"cle\" expected"; break;
			case 18: s = "\"clt\" expected"; break;
			case 19: s = "\"cne\" expected"; break;
			case 20: s = "\"div\" expected"; break;
			case 21: s = "\"halt\" expected"; break;
			case 22: s = "\"inpb\" expected"; break;
			case 23: s = "\"inpi\" expected"; break;
			case 24: s = "\"ldv\" expected"; break;
			case 25: s = "\"ldxa\" expected"; break;
			case 26: s = "\"mul\" expected"; break;
			case 27: s = "\"neg\" expected"; break;
			case 28: s = "\"nop\" expected"; break;
			case 29: s = "\"not\" expected"; break;
			case 30: s = "\"or\" expected"; break;
			case 31: s = "\"prnb\" expected"; break;
			case 32: s = "\"prni\" expected"; break;
			case 33: s = "\"prnl\" expected"; break;
			case 34: s = "\"rem\" expected"; break;
			case 35: s = "\"sto\" expected"; break;
			case 36: s = "\"sub\" expected"; break;
			case 37: s = "\"lda_0\" expected"; break;
			case 38: s = "\"lda_1\" expected"; break;
			case 39: s = "\"ldc_0\" expected"; break;
			case 40: s = "\"ldc_1\" expected"; break;
			case 41: s = "\"inc\" expected"; break;
			case 42: s = "\"dec\" expected"; break;
			case 43: s = "\"dsp\" expected"; break;
			case 44: s = "\"ldc\" expected"; break;
			case 45: s = "\"lda\" expected"; break;
			case 46: s = "\"ldl\" expected"; break;
			case 47: s = "\"stl\" expected"; break;
			case 48: s = "\"prns\" expected"; break;
			case 49: s = "\"brn\" expected"; break;
			case 50: s = "\"bze\" expected"; break;
			case 51: s = "??? expected"; break;
			case 52: s = "invalid Statement"; break;
			case 53: s = "invalid OneWord"; break;
			case 54: s = "invalid TwoWord"; break;
			case 55: s = "invalid TwoWord"; break;
			case 56: s = "invalid Branch"; break;
			case 57: s = "invalid Branch"; break;

			default: s = "error " + n; break;
		}
		StoreError(line, col, s);
		count++;
	}

	public static void SemErr (int line, int col, int n) {
		StoreError(line, col, ("error " + n));
		count++;
	}

	public static void Error (int line, int col, string s) {
		StoreError(line, col, s);
		count++;
	}

	public static void Error (string s) {
		if (mergeErrors) mergedList.WriteLine(s); else Console.WriteLine(s);
		count++;
	}

	public static void Warn (int line, int col, string s) {
		StoreError(line, col, s);
		warns++;
	}

	public static void Warn (string s) {
		if (mergeErrors) mergedList.WriteLine(s); else Console.WriteLine(s);
		warns++;
	}

	public static void Exception (string s) {
		Console.WriteLine(s);
		System.Environment.Exit(1);
	}

} // end Errors

} // end namespace

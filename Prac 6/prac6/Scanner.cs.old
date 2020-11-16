
using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Assem {

public class Token {
	public int kind;    // token kind
	public int pos;     // token position in the source text (starting at 0)
	public int col;     // token column (starting at 0)
	public int line;    // token line (starting at 1)
	public string val;  // token value
	public Token next;  // AW 2003-03-07 Tokens are kept in linked list
}

public class Buffer {
	public const char EOF = (char)256;
	static byte[] buf;
	static int bufLen;
	static int pos;

	public static void Fill (Stream s) {
		bufLen = (int) s.Length;
		buf = new byte[bufLen];
		s.Read(buf, 0, bufLen);
		pos = 0;
	}

	public static int Read () {
		if (pos < bufLen) return buf[pos++];
		else return EOF;                          /* pdt */
	}

	public static int Peek () {
		if (pos < bufLen) return buf[pos];
		else return EOF;                          /* pdt */
	}

	/* AW 2003-03-10 moved this from ParserGen.cs */
	public static string GetString (int beg, int end) {
		StringBuilder s = new StringBuilder(64);
		int oldPos = Buffer.Pos;
		Buffer.Pos = beg;
		while (beg < end) { s.Append((char)Buffer.Read()); beg++; }
		Buffer.Pos = oldPos;
		return s.ToString();
	}

	public static int Pos {
		get { return pos; }
		set {
			if (value < 0) pos = 0;
			else if (value >= bufLen) pos = bufLen;
			else pos = value;
		}
	}

} // end Buffer

public class Scanner {
	const char EOL = '\n';
	const int  eofSym = 0;
	const int charSetSize = 256;
	const int maxT = 51;
	const int noSym = 51;
	// terminals
	const int EOF_SYM = 0;
	const int identifier_Sym = 1;
	const int number_Sym = 2;
	const int label_Sym = 3;
	const int stringLit_Sym = 4;
	const int Comment_Sym = 5;
	const int EOL_Sym = 6;
	const int assem_Sym = 7;
	const int begin_Sym = 8;
	const int end_Sym = 9;
	const int point_Sym = 10;
	const int add_Sym = 11;
	const int and_Sym = 12;
	const int anew_Sym = 13;
	const int ceq_Sym = 14;
	const int cge_Sym = 15;
	const int cgt_Sym = 16;
	const int cle_Sym = 17;
	const int clt_Sym = 18;
	const int cne_Sym = 19;
	const int div_Sym = 20;
	const int halt_Sym = 21;
	const int inpb_Sym = 22;
	const int inpi_Sym = 23;
	const int ldv_Sym = 24;
	const int ldxa_Sym = 25;
	const int mul_Sym = 26;
	const int neg_Sym = 27;
	const int nop_Sym = 28;
	const int not_Sym = 29;
	const int or_Sym = 30;
	const int prnb_Sym = 31;
	const int prni_Sym = 32;
	const int prnl_Sym = 33;
	const int rem_Sym = 34;
	const int sto_Sym = 35;
	const int sub_Sym = 36;
	const int ldaunderscore0_Sym = 37;
	const int ldaunderscore1_Sym = 38;
	const int ldcunderscore0_Sym = 39;
	const int ldcunderscore1_Sym = 40;
	const int inc_Sym = 41;
	const int dec_Sym = 42;
	const int dsp_Sym = 43;
	const int ldc_Sym = 44;
	const int lda_Sym = 45;
	const int ldl_Sym = 46;
	const int stl_Sym = 47;
	const int prns_Sym = 48;
	const int brn_Sym = 49;
	const int bze_Sym = 50;
	const int NOT_SYM = 51;
	// pragmas

	static short[] start = {
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  7,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  1,  9,  0,
	  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  0,  6,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8, 14,  8,  8,  8,
	  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  -1};
	static char valCh;       // current input character (for token.val)

	static Token t;          // current token
	static char ch;          // current input character
	static int pos;          // column number of current character
	static int line;         // line number of current character
	static int lineStart;    // start position of current line
	static int oldEols;      // EOLs that appeared in a comment;
	static BitArray ignore;  // set of characters to be ignored by the scanner

	static Token tokens;     // the complete input token stream
	static Token pt;         // current peek token

	public static void Init (string fileName) {
		FileStream s = null;
		try {
			s = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			Init(s);
		} catch (IOException) {
			Console.WriteLine("--- Cannot open file {0}", fileName);
			System.Environment.Exit(1);
		} finally {
			if (s != null) s.Close();
		}
	}

	public static void Init (Stream s) {
		Buffer.Fill(s);
		pos = -1; line = 1; lineStart = 0;
		oldEols = 0;
		NextCh();
		ignore = new BitArray(charSetSize+1);
		ignore[' '] = true;  // blanks are always white space
		ignore[9] = true; ignore[10] = true; ignore[11] = true; ignore[12] = true; 
		ignore[13] = true; 
		//--- AW: fill token list
		tokens = new Token();  // first token is a dummy
		Token node = tokens;
		do {
			node.next = NextToken();
			node = node.next;
		} while (node.kind != eofSym);
		node.next = node;
		node.val = "EOF";
		t = pt = tokens;
	}

	static void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; }
		else {
			ch = (char)Buffer.Read(); pos++;
			// replace isolated '\r' by '\n' in order to make
			// eol handling uniform across Windows, Unix and Mac
			if (ch == '\r' && Buffer.Peek() != '\n') ch = EOL;
			if (ch == EOL) { line++; lineStart = pos + 1; }
		}
		valCh = ch;
		if (ch != Buffer.EOF) ch = char.ToLower(ch);
	}


	static bool Comment0() {
		int level = 1, line0 = line, lineStart0 = lineStart;
		NextCh();
			for(;;) {
				if (ch == 10) {
					level--;
					if (level == 0) { oldEols = line - line0; NextCh(); return true; }
					NextCh();
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
	}

	static bool Comment1() {
		int level = 1, line0 = line, lineStart0 = lineStart;
		NextCh();
			for(;;) {
				if (ch == '}') {
					level--;
					if (level == 0) { oldEols = line - line0; NextCh(); return true; }
					NextCh();
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
	}


	static void CheckLiteral() {
		switch (t.val.ToLower()) {
			case "assem": t.kind = assem_Sym; break;
			case "begin": t.kind = begin_Sym; break;
			case "end": t.kind = end_Sym; break;
			case "add": t.kind = add_Sym; break;
			case "and": t.kind = and_Sym; break;
			case "anew": t.kind = anew_Sym; break;
			case "ceq": t.kind = ceq_Sym; break;
			case "cge": t.kind = cge_Sym; break;
			case "cgt": t.kind = cgt_Sym; break;
			case "cle": t.kind = cle_Sym; break;
			case "clt": t.kind = clt_Sym; break;
			case "cne": t.kind = cne_Sym; break;
			case "div": t.kind = div_Sym; break;
			case "halt": t.kind = halt_Sym; break;
			case "inpb": t.kind = inpb_Sym; break;
			case "inpi": t.kind = inpi_Sym; break;
			case "ldv": t.kind = ldv_Sym; break;
			case "ldxa": t.kind = ldxa_Sym; break;
			case "mul": t.kind = mul_Sym; break;
			case "neg": t.kind = neg_Sym; break;
			case "nop": t.kind = nop_Sym; break;
			case "not": t.kind = not_Sym; break;
			case "or": t.kind = or_Sym; break;
			case "prnb": t.kind = prnb_Sym; break;
			case "prni": t.kind = prni_Sym; break;
			case "prnl": t.kind = prnl_Sym; break;
			case "rem": t.kind = rem_Sym; break;
			case "sto": t.kind = sto_Sym; break;
			case "sub": t.kind = sub_Sym; break;
			case "inc": t.kind = inc_Sym; break;
			case "dec": t.kind = dec_Sym; break;
			case "dsp": t.kind = dsp_Sym; break;
			case "ldc": t.kind = ldc_Sym; break;
			case "lda": t.kind = lda_Sym; break;
			case "ldl": t.kind = ldl_Sym; break;
			case "stl": t.kind = stl_Sym; break;
			case "prns": t.kind = prns_Sym; break;
			case "brn": t.kind = brn_Sym; break;
			case "bze": t.kind = bze_Sym; break;
			default: break;
		}
	}

	/* AW Scan() renamed to NextToken() */
	static Token NextToken() {
		while (ignore[ch]) NextCh();
		if (ch == ';' && Comment0() ||ch == '{' && Comment1()) return NextToken();
		t = new Token();
		t.pos = pos; t.col = pos - lineStart + 1; t.line = line;
		int state = start[ch];
		StringBuilder buf = new StringBuilder(16);
		buf.Append(valCh); NextCh();
		switch (state) {
			case -1: { t.kind = eofSym; goto done; } // NextCh already done /* pdt */
			case 0: { t.kind = noSym; goto done; }   // NextCh already done
			case 1:
				if ((ch >= '0' && ch <= '9')) { buf.Append(valCh); NextCh(); goto case 2; }
				else { t.kind = noSym; goto done; }
			case 2:
				if ((ch >= '0' && ch <= '9')) { buf.Append(valCh); NextCh(); goto case 2; }
				else { t.kind = number_Sym; goto done; }
			case 3:
				{ t.kind = label_Sym; goto done; }
			case 4:
				if ((ch >= ' ' && ch <= '!'
				  || ch >= '#' && ch <= 255)) { buf.Append(valCh); NextCh(); goto case 4; }
				else if (ch == '"') { buf.Append(valCh); NextCh(); goto case 5; }
				else { t.kind = noSym; goto done; }
			case 5:
				{ t.kind = stringLit_Sym; goto done; }
			case 6:
				if (!(ch == 10) && ch != Buffer.EOF) { buf.Append(valCh); NextCh(); goto case 6; }
				else { t.kind = Comment_Sym; goto done; }
			case 7:
				{ t.kind = EOL_Sym; goto done; }
			case 8:
				if ((ch >= '0' && ch <= '9'
				  || ch >= 'a' && ch <= 'z')) { buf.Append(valCh); NextCh(); goto case 8; }
				else if (ch == ':') { buf.Append(valCh); NextCh(); goto case 3; }
				else { t.kind = identifier_Sym; t.val = buf.ToString(); CheckLiteral(); return t; }
			case 9:
				{ t.kind = point_Sym; goto done; }
			case 10:
				{ t.kind = ldaunderscore0_Sym; goto done; }
			case 11:
				{ t.kind = ldaunderscore1_Sym; goto done; }
			case 12:
				{ t.kind = ldcunderscore0_Sym; goto done; }
			case 13:
				{ t.kind = ldcunderscore1_Sym; goto done; }
			case 14:
				if ((ch >= '0' && ch <= '9'
				  || ch >= 'a' && ch <= 'c'
				  || ch >= 'e' && ch <= 'z')) { buf.Append(valCh); NextCh(); goto case 8; }
				else if (ch == ':') { buf.Append(valCh); NextCh(); goto case 3; }
				else if (ch == 'd') { buf.Append(valCh); NextCh(); goto case 15; }
				else { t.kind = identifier_Sym; t.val = buf.ToString(); CheckLiteral(); return t; }
			case 15:
				if ((ch >= '0' && ch <= '9'
				  || ch == 'b'
				  || ch >= 'd' && ch <= 'z')) { buf.Append(valCh); NextCh(); goto case 8; }
				else if (ch == ':') { buf.Append(valCh); NextCh(); goto case 3; }
				else if (ch == 'a') { buf.Append(valCh); NextCh(); goto case 16; }
				else if (ch == 'c') { buf.Append(valCh); NextCh(); goto case 17; }
				else { t.kind = identifier_Sym; t.val = buf.ToString(); CheckLiteral(); return t; }
			case 16:
				if ((ch >= '0' && ch <= '9'
				  || ch >= 'a' && ch <= 'z')) { buf.Append(valCh); NextCh(); goto case 8; }
				else if (ch == ':') { buf.Append(valCh); NextCh(); goto case 3; }
				else if (ch == '_') { buf.Append(valCh); NextCh(); goto case 18; }
				else { t.kind = identifier_Sym; t.val = buf.ToString(); CheckLiteral(); return t; }
			case 17:
				if ((ch >= '0' && ch <= '9'
				  || ch >= 'a' && ch <= 'z')) { buf.Append(valCh); NextCh(); goto case 8; }
				else if (ch == ':') { buf.Append(valCh); NextCh(); goto case 3; }
				else if (ch == '_') { buf.Append(valCh); NextCh(); goto case 19; }
				else { t.kind = identifier_Sym; t.val = buf.ToString(); CheckLiteral(); return t; }
			case 18:
				if (ch == '0') { buf.Append(valCh); NextCh(); goto case 10; }
				else if (ch == '1') { buf.Append(valCh); NextCh(); goto case 11; }
				else { t.kind = noSym; goto done; }
			case 19:
				if (ch == '0') { buf.Append(valCh); NextCh(); goto case 12; }
				else if (ch == '1') { buf.Append(valCh); NextCh(); goto case 13; }
				else { t.kind = noSym; goto done; }

		}
		done:
		t.val = buf.ToString();
		return t;
	}

	/* AW 2003-03-07 get the next token, move on and synch peek token with current */
	public static Token Scan () {
		t = pt = t.next;
		return t;
	}

	/* AW 2003-03-07 get the next token, ignore pragmas */
	public static Token Peek () {
		do {                      // skip pragmas while peeking
			pt = pt.next;
		} while (pt.kind > maxT);
		return pt;
	}

	/* AW 2003-03-11 to make sure peek start at current scan position */
	public static void ResetPeek () { pt = t; }

} // end Scanner

} // end namespace

  //Luke Wilkinson: G16W4712
  //Scanner & Parser

  // This is a skeleton program for developing a parser for C declarations
  // P.D. Terry, Rhodes University, 2015

  using Library;
  using System;
  using System.Text;

  class Token {
    public int kind;
    public string val;

    public Token(int kind, string val) {
      this.kind = kind;
      this.val = val;
    } // constructor

  } // Token

  class Declarations {

    // +++++++++++++++++++++++++ File Handling and Error handlers ++++++++++++++++++++

    static InFile input;
    static OutFile output;

    static string NewFileName(string oldFileName, string ext) {
    // Creates new file name by changing extension of oldFileName to ext
      int i = oldFileName.LastIndexOf('.');
      if (i < 0) return oldFileName + ext; else return oldFileName.Substring(0, i) + ext;
    } // NewFileName

    static void ReportError(string errorMessage) {
    // Displays errorMessage on standard output and on reflected output
      Console.WriteLine(errorMessage);
      output.WriteLine(errorMessage);
    } // ReportError

    static void Abort(string errorMessage) {
    // Abandons parsing after issuing error message
      ReportError(errorMessage);
      output.Close();
      System.Environment.Exit(1);
    } // Abort

    // +++++++++++++++++++++++  token kinds enumeration +++++++++++++++++++++++++

    const int
      noSym        = 0,
      intSym       = 1,
      charSym      = 2,
      voidSym      = 3,
      numSym       = 4,
      identSym     = 5,
      lparenSym    = 6,
      rparenSym    = 7,
      lbrackSym    = 8,
      rbrackSym    = 9,
      commaSym     = 10,
      semicolonSym = 11,
      EOFSym       = 12;
      // and others like this

    // +++++++++++++++++++++++++++++ Character Handler ++++++++++++++++++++++++++

    const char EOF = '\0';
    static bool atEndOfFile = false;

    // Declaring ch as a global variable is done for expediency - global variables
    // are not always a good thing

    static char ch;    // look ahead character for scanner

    static void GetChar() {
    // Obtains next character ch from input, or CHR(0) if EOF reached
    // Reflect ch to output
      if (atEndOfFile) ch = EOF;
      else {
        ch = input.ReadChar();
        atEndOfFile = ch == EOF;
        if (!atEndOfFile) output.Write(ch);
      }
    } // GetChar

    // +++++++++++++++++++++++++++++++ Scanner ++++++++++++++++++++++++++++++++++

    // Declaring sym as a global variable is done for expediency - global variables
    // are not always a good thing

    static Token sym;

    static void GetSym() {
    // Scans for next sym from input
      while (ch > EOF && ch <= ' ') GetChar();
      StringBuilder symLex = new StringBuilder();
      int symKind = noSym;

      // over to you!
      if (Char.IsLetter(ch)) { 
        do {
          symLex.Append(ch); 
          GetChar();
        } while (Char.IsLetterOrDigit(ch) || ch == '_');
        //some specials.
        if (symLex.ToString() == "void") symKind = voidSym;
        else if (symLex.ToString() == "int") symKind = intSym;
        else if (symLex.ToString() == "char") symKind = charSym;
        else symKind = identSym;
      }
      else if (Char.IsDigit(ch)) {
        do {
          symLex.Append(ch); 
          GetChar();
        } while (Char.IsDigit(ch));
        symKind = numSym;
      }
      else {
        symLex.Append(ch);
        switch (ch) {
          case EOF:
            symLex = new StringBuilder("EOF");   // special representation
            symKind = EOFSym;   // End of file. Don't get char.
            break;             
          case '(':
            symKind = lparenSym; 
            GetChar(); 
            break;
          case ')':
            symKind = rparenSym; 
            GetChar(); 
            break;
          case '[':
            symKind = lbrackSym; 
            GetChar(); 
            break;
          case ']':
            symKind = rbrackSym; 
            GetChar(); 
            break;
          case ',':
            symKind = commaSym; 
            GetChar(); 
            break;
          case ';':
            symKind = semicolonSym; 
            GetChar(); 
            break;
          default :
            symKind = noSym; 
            GetChar(); 
            break;
        }
      }

      sym = new Token(symKind, symLex.ToString());
    } // GetSym


    //+++++++++++++++++++++++++++++++ Parser +++++++++++++++++++++++++++++++++++

    /* I don't actually need intSet???
    static IntSet //dont know if 1stDec's params are corrrect
    typeSet = new IntSet(voidSym, intSym, charSym), 
    endSet = new IntSet(EOFSym, semicolonSym);
    //Trash Code
    */

    static void Accept(int wantedSym, string errorMessage) {
    // Checks that lookahead token is wantedSym
      if (sym.kind == wantedSym) GetSym(); else Abort(errorMessage);
    } // Accept

    static void Accept(IntSet allowedSet, string errorMessage) {
    // Checks that lookahead token is in allowedSet
      if (allowedSet.Contains(sym.kind)) GetSym(); else Abort(errorMessage);
    } // Accept

    //+++++++++++++++++++++++++++++++ PRODUCTIONS +++++++++++++++++++++++++++++++
    //CDecls = {DecList} EOF .
    static void CDecls() {
      //{
      while (sym.kind != EOFSym) {
        DecList(); 
        GetSym();
      }
      //}
      Accept(EOFSym, "EOF expected");
    }

    //DecList = Type OneDecl {"," OneDecl} ";" .
    static void DecList() {
      Type();
      OneDecl();
      //{
      while (sym.kind == commaSym) {
        GetSym();
        OneDecl();
      }
      //}
      Accept(semicolonSym, "; expected but was, " + sym.val);
    }

    //Type = "int" | "void" | "char" .
    static void Type() {  
      switch (sym.kind) {
        case intSym:
          GetSym(); 
          break;
        //  |
        case voidSym:
          GetSym(); 
          break;
        //  |
        case charSym:
          GetSym(); 
          break;
        default:
          break;
      }
    }
    
    //OneDecl = ident [Suffix] .
    static void OneDecl() {
      if (sym.kind == identSym)
      {
        //[
        GetSym();  
        if (sym.kind == lparenSym || sym.kind == lbrackSym) {
          Suffix();
        }
        //]
      } else {
        Abort("identifier expected but was, " + sym.val);
      }
    }
        
    //Suffix = Array {Array} | Params.
    static void Suffix() {
      switch (sym.kind) {
        case lbrackSym:
          GetSym();
          Array();
          //{
          while (sym.kind == lbrackSym) {
            GetSym();
            Array();
          }
          //}
          //  |
          break;
        case lparenSym:
          Params();
          break;
        default:
          Abort("invalid start to Suffix");
          break;
      }
    }
            
    //Params = "(" [OneParam {"," OneParam}] ")" .
    static void Params() {
      Accept(lparenSym, "( expected but was, " + sym.val);    
      //[
      if (sym.kind == intSym || sym.kind == voidSym || sym.kind == charSym) {
        OneParam();
        //{   
        while (sym.kind == commaSym) {   
          GetSym();                        
          OneParam();                               
        }
        //}
      }
      //]
      Accept(rparenSym, ") expected but was, " + sym.val);            
    }

    //OneParam = Type [OneDecl] .
    static void OneParam() {
      Type();
      //[
      if(sym.kind == identSym) OneDecl();
      else Abort("identifier expected but was, " + sym.val);
      //]
    }

    //Array = "[" [number] "]" .
    static void Array() {
      //[
      if(sym.kind == numSym) Accept(numSym, "Invalid number");
      //]
      Accept(rbrackSym, "] expected but was, " + sym.val);
    }


    // +++++++++++++++++++++ Main driver function +++++++++++++++++++++++++++++++ 
    //END OF PARSER 

    public static void Main(string[] args) {
      // Open input and output files from command line arguments
      if (args.Length == 0) {
        Console.WriteLine("Usage: Declarations FileName");
        System.Environment.Exit(1);
      }
      input = new InFile(args[0]);
      output = new OutFile(NewFileName(args[0], ".out"));

      GetChar();                                  // Lookahead character

    /*  To test the scanner we can use a loop like the following:

      do {
        GetSym();                                 // Lookahead symbol
        OutFile.StdOut.Write(sym.kind, 3);
        OutFile.StdOut.WriteLine(" " + sym.val);  // See what we got
      } while (sym.kind != EOFSym);

    */  
    //After the scanner is debugged we shall substitute this code:

      GetSym();                                   // Lookahead symbol
      CDecls();                                   // Start to parse from the goal symbol
      // if we get back here everything must have been satisfactory
      Console.WriteLine("Parsed correctly");

    //
      output.Close();
    } // Main

  } // Declarations


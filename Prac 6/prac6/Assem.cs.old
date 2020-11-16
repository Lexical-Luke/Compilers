
/* Driver for PVM compilers and assemblers: Coco/R for C#.
   P.D. Terry, Rhodes University, 2015
   2015/10/05 For Practical 6 
   
   Luke Wilkinson: G16W4712
   */

//  ----------------------- you may need to change the "using" clauses:

using Library;
using System;
using System.IO;
using System.Text;
//using Table.cs;
//using assem/Table.cs;

namespace Assem {

  public class Assem {

    private static string newFileName(string s, string ext) {
      int i = s.LastIndexOf('.');
      if (i < 0) return s + ext; else return s.Substring(0, i) + ext;
    }

    public static void Main (string[] args) {
      bool mergeErrors = false;
      bool immediate   = false;
      string inputName = null;

      // ------------------------ process command line parameters:

      Console.WriteLine("Assem compiler 1.00");

      for (int i = 0; i < args.Length; i++) {
             if (args[i].ToLower().Equals("-l")) mergeErrors = true;
        else if (args[i].ToLower().Equals("-i")) immediate   = true;
        else inputName = args[i];
      }
      if (inputName == null) {
        Console.WriteLine("No input file specified");
        Console.WriteLine("Usage: Parva [-l] source.pav [-l]");
        Console.WriteLine("-l directs source listing to listing.txt");
        Console.WriteLine("-i proceeds directly to interpreter");
        System.Environment.Exit(1);
      }

      // ------------------------ parser and scanner initialization

      int pos = inputName.LastIndexOf('/');
      if (pos < 0) pos = inputName.LastIndexOf('\\');
      string dir = inputName.Substring(0, pos+1);

      string outputName = null;
      pos = inputName.LastIndexOf('.');
      if (pos < 0) outputName = inputName + ".pretty";
      else outputName = inputName.Substring(0, pos) + ".pretty";
      Parser.pretty = new OutFile(outputName);
      if (Parser.pretty.OpenError()) {
        Console.WriteLine("cannot open " + outputName);
        System.Environment.Exit(1);
      }

      Scanner.Init(inputName);
      Errors.Init(inputName, dir, mergeErrors);
      PVM.Init();
      //Table.Init();

      // ------------------------ compilation

      Parser.Parse();
      Errors.Summarize();

      // ------------------------ interpretation

      bool assembledOK = Parser.Successful();
      int initSP = CodeGen.GetInitSP();
      string codeName = newFileName(inputName, ".cod");
      int codeLength = CodeGen.GetCodeLength();
      PVM.ListCode(codeName, codeLength);
      if (!assembledOK || codeLength == 0) {
        Console.WriteLine("Unable to interpret code");
        System.Environment.Exit(1);
      }
      else {
        if (immediate) PVM.QuickInterpret(codeLength, initSP);
        char reply;
        do {
          Console.Write("\n\nInterpret [y/N]? ");
          reply = (Console.ReadLine() + " ").ToUpper()[0];
          if (reply == 'Y') PVM.Interpret(codeLength, initSP);
        } while (reply == 'Y');
      }
    }

  } // end Parva

} // end namespace

// Handle label table for simple PVM assembler
// P.D. Terry, Rhodes University, 2015
//Luke Wilkinson: G16W4712

using Library;
using System.Collections.Generic;
using System;

namespace Assem {

  class LabelEntry {

    public string name;
    public Label label;
    public List<Int32> refs = null;

    public LabelEntry(string name, Label label, int lineNumber) {
      this.name  = name;
      this.label = label;
      this.refs = new List<Int32>();
      this.refs.Add(lineNumber);
    }

    public void AddReference(int lineNumber) { 
      this.refs.Add(lineNumber);
    } // AddReference

  } // end LabelEntry

// -------------------------------------------------------------------------------------

  class LabelTable {

    private static List<LabelEntry> list = new List<LabelEntry>();

    public static void Insert(LabelEntry entry) {
    // Inserts entry into label table
      list.Add(entry);
    } // insert

    public static LabelEntry Find(string name) {
    // Searches table for label entry matching name.  If found then returns entry.
    // If not found, returns null
      int i = 0;
      while (i < list.Count && !name.Equals(list[i].name)) i++;
      if (i >= list.Count) return null; 
      else return list[i];
    } // find

    public static void CheckLabels() {
    // Checks that all labels have been defined (no forward references outstanding)
      for (int i = 0; i < list.Count; i++) {
        if (!list[i].label.IsDefined())
          Parser.SemError("undefined label - " + list[i].name);
      }
    } // CheckLabels

    public static void ListReferences(OutFile output) {
    // Cross reference list of all labels used on output file
      //IO.WriteLine("Labels:\n");
      output.WriteLine("Labels:\n");

      for (int i = 0; i < list.Count; i++){

        string reflist = list[i].name;

        if (list[i].label.IsDefined())
          reflist += "  (defined) ";
        foreach (int r in list[i].refs)
            reflist += " " + r;

        //IO.WriteLine(reflist);
        output.WriteLine(reflist);
      }
    } // ListReferences

  } // end LabelTable

// -------------------------------------------------------------------------------------

  class VariableEntry {

    public string name;
    public int offset;
    public List<Int32> refs = null;

    public VariableEntry(string name, int offset, int lineNumber) {
      this.name   = name;
      this.offset = offset;
      this.refs = new List<Int32>();
      refs.Add(offset);
      this.refs.Add(lineNumber);
    }

    public void AddReference(int lineNumber) { 
      this.refs.Add(lineNumber);
    } // AddReference

  } // end VariableEntry

// -------------------------------------------------------------------------------------

  class VarTable {

    private static List<VariableEntry> list = new List<VariableEntry>();
    private static int varOffset = 0;

    public static int FindOffset(string name,  int lineNumber) {
    // Searches table for variable entry matching name.  If found then returns the known offset.
    // If not found, makes an entry and updates the master offset
      int i = 0;
      while (i < list.Count && !name.Equals(list[i].name)) i++;      
      if (i >= list.Count) { 
        list.Add(new VariableEntry(name, varOffset, lineNumber)); 	
        return varOffset++;
      } else { //variable already assigned
		    list[i].AddReference(lineNumber);
        return list[i].offset;
      } 
    } // FindOffset

    public static void ListReferences(OutFile output) {
    // Cross reference list of all variables on output file
      //IO.WriteLine("\nVariables:\n\n");
      output.WriteLine("\n\nVariables:\n");
      for (int i = 0; i < list.Count; i++){
        string reflist = list[i].name;
        reflist += "  - offset ";

        foreach (int r in list[i].refs)
            reflist += " " + r;

        //IO.WriteLine(reflist);
        output.WriteLine(reflist);
      }
    } // ListReferences

  } // end VarTable

} // end namespace

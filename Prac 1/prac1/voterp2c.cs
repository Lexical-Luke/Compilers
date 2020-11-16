using Library;

class voter {
  
  static public void Main(string[] args) {
    const int votingAge = 18;
    const bool overTheHill = true;
    int age, eligible = 0, total = 0;
    bool allEligible = true;
    int[] voters = new int[100];
    { IO.Write("Supply ages "); age = IO.ReadInt(); }
    while (age > 0) {
      bool canVote = age > votingAge;
      allEligible = allEligible && canVote;
      if (canVote) {
        voters[eligible] = age;
        eligible = eligible + 1;
        total = total + voters[eligible - 1];
      }
      { age = IO.ReadInt(); }
    }
    { IO.Write(eligible); IO.Write(" voters.  Average age is "); IO.Write(total / eligible); IO.Write("\n"); }
    if (allEligible)
      { IO.Write("Everyone was above voting age"); }
  } // Main
  
} // voter
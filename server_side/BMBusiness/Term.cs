using BMData;

namespace BMBusiness
{
    public class Term
    {

        private Mode mode = Mode.Add;

        public int TermID { get; set; }
        public string Description{ get; set; }
        public TermDTO TDTO { get { return new TermDTO(TermID, Description); } }

        public Term(TermDTO termDTO, Mode mode = Mode.Add)
        {
            TermID = termDTO.TermID;
            Description = termDTO.Description;

            this.mode = mode;
        }

        public static List<TermDTO>? Terms() => TermDB.GetAllTerms();

        public static Term? Find(int termID)
            => TermDB.GetTermByID(termID) is TermDTO termDTO ? new Term(termDTO, Mode.Update) : null;

        public bool Delete() => TermDB.DeleteTerm(TermID);

        private bool AddNew()
        {
            TermID = TermDB.AddNewTerm(TDTO);
            return TermID != -1;
        }

        private bool Update() => TermDB.UpdateTerm(TDTO);

        public bool Save()
        {
            switch (mode)
            {
                case Mode.Add:
                    if (AddNew())
                    {
                        mode = Mode.Update;
                        return true;
                    }
                    else return false;

                case Mode.Update:
                    return Update();
            }

            return false;
        }
    }
}

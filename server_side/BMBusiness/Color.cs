using BMData;

namespace BMBusiness
{
    public class Color
    {
        private Mode mode = Mode.Add;

        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public ColorDTO CDTO { get { return new ColorDTO(ColorID, ColorName); } }

        //  --- Constructor.
        public Color(ColorDTO colorDTO, Mode mode = Mode.Add)
        {
            ColorID = colorDTO.ColorID;
            ColorName = colorDTO.ColorName;

            this.mode = mode;
        }


        public static List<ColorDTO>? Colors() => ColorDB.GetAllColors();

        public static Color? Find(int colorID)
            => ColorDB.GetColorByID(colorID) is ColorDTO colorDTO ? new Color(colorDTO, Mode.Update) : null;

        public static Color? Find(string colorName)
            => ColorDB.GetColorByName(colorName) is ColorDTO colorDTO ? new Color(colorDTO, Mode.Update) : null;

        public static bool IsExists(string colorName) => ColorDB.IsExists(colorName);

        public bool Delete() => ColorDB.DeleteColor(ColorID);

        private bool AddNew()
        {
            ColorID = ColorDB.AddNewColor(CDTO);
            return ColorID != -1;
        }
        private bool Update() => ColorDB.UpdateColor(CDTO);

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

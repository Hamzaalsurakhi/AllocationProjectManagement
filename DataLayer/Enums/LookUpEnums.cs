namespace DataLayer.Enums
{
    public static class LookUpEnums
    {
        public enum CategoryCode
        {
            Position = 1,
            Level = 2,
            Status = 3,
            TeamCountry = 4,
        }
        public enum PositionCategory
        {
            FullStack = 10,
            Frontend = 11,
            Backend = 12,
            AI = 13,
            UX = 14,
            QA = 15,
            BusinessAnalyst = 16,
            AssistantManager = 17,
        }
        public enum levelCategory
        {
            Fresh = 31,
            Junior = 32,
            MidSeniorLevel = 33,
            Senior = 34,
            TeamLeader = 35,
            ProjectManager = 36,
        }
        public enum StatusCategory
        {
            Active = 41,
            OnHold = 42,
            Design = 43,
            Deployment = 45,
            Testing = 46,
            Development = 47,
            Finished = 48,
        }
        public enum TeamCountryCategory
        {
            TeamJordan = 51,
            TeamSaudiArabia = 52,
            TeamEgypt = 53,
        }
    }
}

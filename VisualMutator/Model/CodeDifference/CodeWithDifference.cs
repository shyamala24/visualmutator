namespace CommonUtilityInfrastructure.Comparers
{
    using System.Collections.Generic;

    public class CodeWithDifference
    {
        public string Code
        {
            get;
            set;
        }
        public List<LineChange> LineChanges
        {
            get;
            set;
        }
    }
}
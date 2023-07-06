using System.Collections.Generic;

namespace DialogueSystem.Data.Error
{
    using Elements;
    public class D_GroupErrorData
    {
        public D_ErrorData ErrorData { get; set; }
        public List<Dialogue_Group> Groups { get; set; }

        public D_GroupErrorData()
        {
            ErrorData = new D_ErrorData();
            Groups = new List<Dialogue_Group>();
        }
    }
}

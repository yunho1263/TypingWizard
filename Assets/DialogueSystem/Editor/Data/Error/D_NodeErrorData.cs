using System.Collections.Generic;

namespace DialogueSystem.Data.Error
{
    using Elements;
    public class D_NodeErrorData
    {
        public D_ErrorData ErrorData { get; set; }
        public List<Dialogue_Node> Nodes { get; set; }

        public D_NodeErrorData()
        {
            ErrorData = new D_ErrorData();
            Nodes = new List<Dialogue_Node>();
        }
    }
}

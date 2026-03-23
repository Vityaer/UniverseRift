using System.Collections.Generic;

namespace Models.Dialogs
{
    public class DialogModel : BaseModel
    {
        public List<PersonalStatement> Statements = new List<PersonalStatement>();
    }
}
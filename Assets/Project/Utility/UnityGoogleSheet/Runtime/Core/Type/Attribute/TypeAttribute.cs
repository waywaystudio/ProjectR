namespace UnityGoogleSheet.Core.Type.Attribute
{
    public class TypeAttribute : System.Attribute
    {
        public System.Type type;
        public string[] separactors;
         
        public TypeAttribute(System.Type Type)
        { 
            type = Type;
            separactors = new [] { Type.Name };
        }
        public TypeAttribute(System.Type Type, params string[] TypeName)
        { 
            type = Type;
            separactors = TypeName;
        }
    }
}
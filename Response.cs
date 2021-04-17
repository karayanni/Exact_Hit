
namespace Exact_Hit
{
    public class Response
    {
        public int ExactHits;

        public int AlmostHits;

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Response newResponse = (Response)obj;
                
                return newResponse.ExactHits == this.ExactHits && newResponse.AlmostHits == AlmostHits;
            }
        }
    }
}

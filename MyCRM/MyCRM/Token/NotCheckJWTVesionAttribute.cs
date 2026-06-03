namespace MyCRM.Token
{
    
    [AttributeUsage(AttributeTargets.Method)]//限制只能用在方法上
    public class NotCheckJWTVesionAttribute:Attribute
    {
        //本注解为被注解就不检查JWTVersion的合法性

    }
}

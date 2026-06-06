namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class MongoDbFactory : IAbstractFactoryBD
    {
        public IAbstractFactoryBaseDatos CrearMotor()
        {
            return new ConexionMongoDB();
        }
    }
}

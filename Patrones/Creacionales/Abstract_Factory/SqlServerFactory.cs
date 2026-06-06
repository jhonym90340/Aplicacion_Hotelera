namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class SqlServerFactory : IAbstractFactoryBD
    {
        public IAbstractFactoryBaseDatos CrearMotor()
        {
            return new ConexionSQL();
        }
    }
}

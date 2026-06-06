namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class SqliteFactory : IAbstractFactoryBD
    {
        public IAbstractFactoryBaseDatos CrearMotor()
        {
            return new ConexionSQLite();
        }
    }
}

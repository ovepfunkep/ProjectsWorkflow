namespace BusinessLogic.Services.Interfaces
{
    public interface ICRUDService<T,TDTO> where T : class
    {
        public TDTO? GetById(int id);
        public IEnumerable<TDTO> GetAll();
        public TDTO Add(TDTO entity);
        public TDTO Update(TDTO entity);
        public bool Delete(int id);
    }

}
namespace StateCache
{
   public interface StateCache<T>
   {
      void Store(T statefulObject);
      void Restore(T statefulObject);
   }
}

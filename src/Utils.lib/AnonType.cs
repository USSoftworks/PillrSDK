namespace Pillr.Utils;
public static class AnonType
{
  public static Func<T> InferAnonymousType<T>(Func<T> f)
  {
    return f;
  }
}

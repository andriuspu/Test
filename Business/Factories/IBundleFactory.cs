using Data;

namespace Business
{
    public interface IBundleFactory
    {
        Bundle Create(int bundleId);
    }
}
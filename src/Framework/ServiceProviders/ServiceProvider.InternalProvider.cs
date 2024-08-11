using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.ServiceProviders;

public partial class ServiceProvider
{
    private sealed class InternalProvider<TInterface, TClass> : ILazyProvider<TInterface> where TClass : TInterface
    {
        private readonly Func<TInterface> _factoryFunc;
        private bool _wasSet;
        private TInterface? _value;

        [RequiresUnreferencedCode("Uses reflection")]
        public InternalProvider(IInstantiator instantiator)
        {
            _factoryFunc = () => instantiator.Instantiate<TClass>();
        }

        public InternalProvider(Func<TInterface> factoryFunc)
        {
            this._factoryFunc = factoryFunc;
        }

        public InternalProvider(TInterface val)
        {
            _factoryFunc = () => default!;
            SetValue(val);
        }

        public TInterface GetValue()
        {
            if (!_wasSet)
            {
                SetValue(_factoryFunc());
            }

            return _value!;
        }

        public void SetValue(TInterface value)
        {
            _wasSet = true;
            this._value = value;
        }
    }
}

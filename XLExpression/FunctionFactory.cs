using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using XLExpression.Functions;

namespace XLExpression
{
    public class FunctionFactory
    {
        private CompositionContainer _container = null;
        private FunctionFactory()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(FunctionFactory).Assembly));

            _container = new CompositionContainer(catalog);

            _container.ComposeParts(this);
        }

        public static FunctionFactory Instance => new FunctionFactory();

        [ImportMany(typeof(IFunction))]
        private List<Lazy<IFunction, IFunctionMetadata>> _functions = null;

        public IFunction? GetOperator(string name)
        {
            //var func = _container.GetExportedValue<IFunction>(name);

            //return func;

            return _functions.FirstOrDefault(f => f.Metadata.Symbol.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }
    }
}

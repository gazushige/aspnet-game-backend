// RpcDispatcher.cs
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using MyApi.Models;

namespace MyApi.Handlers
{
    public class RpcDispatcher
    {
        private readonly IReadOnlyDictionary<string, Func<JsonElement?, ClaimsPrincipal, ApiDbContext, IConfiguration, Task<JsonElement>>> _handlers;

        public RpcDispatcher(IServiceProvider sp)
        {
            var dict = new Dictionary<string, Func<JsonElement?, ClaimsPrincipal, ApiDbContext, IConfiguration, Task<JsonElement>>>();

            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && t.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              i.GetGenericTypeDefinition() == typeof(IRpcHandler<,>)));

            foreach (var type in handlerTypes)
            {
                var instance = sp.GetRequiredService(type);
                var methodProp = type.GetProperty("Method")!;
                var methodName = (string)methodProp.GetValue(instance)!;
                var handleMethod = type.GetMethod("HandleAsync")!;

                dict[methodName] = (p, u, db, cfg) =>
                    (Task<JsonElement>)handleMethod.Invoke(instance, [p, u, db, cfg])!;
            }

            // 起動時にメソッド名の重複を検出
            var duplicates = dict.GroupBy(h => h.Key).Where(g => g.Count() > 1).ToList();
            if (duplicates.Count != 0)
                throw new InvalidOperationException(
                    $"重複したRPCメソッド名: {string.Join(", ", duplicates.Select(g => g.Key))}");

            _handlers = dict;
        }

        public Task<JsonElement> DispatchAsync(
            string method,
            JsonElement? params_,
            ClaimsPrincipal user,
            ApiDbContext db,
            IConfiguration config)
        {
            if (!_handlers.TryGetValue(method, out var handler))
                throw new RpcException(-32601, "Method not found");

            return handler(params_, user, db, config);
        }
    }
}
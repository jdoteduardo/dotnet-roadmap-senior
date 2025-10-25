using Week01_EFCore.Interfaces;

namespace Week01_EFCore.Repository.Decorators
{
    public class RepositoryLoggingDecorator<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly ILoggerService _logger;

        public RepositoryLoggingDecorator(IRepository<T> repository, ILoggerService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            await _logger.LogInformationAsync($"Buscando todos os registros de {typeof(T).Name}");
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            await _logger.LogInformationAsync($"Buscando {typeof(T).Name} com ID {id}");
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _logger.LogInformationAsync($"Adicionando novo {typeof(T).Name}");
            return await _repository.AddAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await _logger.LogInformationAsync($"Atualizando {typeof(T).Name}");
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _logger.LogInformationAsync($"Removendo {typeof(T).Name} com ID {id}");
            return await _repository.DeleteAsync(id);
        }
    }
}
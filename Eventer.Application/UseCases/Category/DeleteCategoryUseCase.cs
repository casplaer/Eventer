using Eventer.Application.Interfaces.UseCases.Category;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Category
{
    public class DeleteCategoryUseCase : IDeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var categoryToDelete = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);

            if (categoryToDelete == null)
            {
                return false;
            }

            await _unitOfWork.Categories.DeleteAsync(categoryToDelete, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}

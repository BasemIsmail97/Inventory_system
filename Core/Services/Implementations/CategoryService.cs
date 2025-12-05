

namespace Services.Implementations
{
    public class CategoryService(IUnitOfWork unitOfWork ,IMapper mapper) : ICategoryServicecs
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateCategoryAsync( CategoryDto createCatygoryDto)
        {
            createCatygoryDto.CreatedAt = DateTime.Now;
            var category = _mapper.Map<Category>(createCatygoryDto);
          await  _unitOfWork.GetRepository<Category>().AddAsync(category);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int catygoryId)
        {
           var Repo =   _unitOfWork.GetRepository<Category>();
            var category = await Repo.GetByIdAsync(catygoryId);
            if(category is null)
            {
                return false;
            }
            Repo.Delete(category);
           await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCatygoriesAsync()
        { 
            var  Categories = await _unitOfWork.GetRepository<Category>().GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(Categories);
            return categoryDtos;

        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int CatygoryId)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(CatygoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category {CatygoryId} not found");
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<int> UpdateCategoryAsync( CategoryDto updateCatygoryDto)
        {
            var existingCategory = await  _unitOfWork.GetRepository<Category>().GetByIdAsync(updateCatygoryDto.Id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category {updateCatygoryDto.Id} not found");
            }
            updateCatygoryDto.UpdatedAt = DateTime.Now;
            var category = _mapper.Map<Category>(updateCatygoryDto);
             _unitOfWork.GetRepository<Category>().Update(category);
            return await _unitOfWork.SaveChangesAsync();
        }

        
    }
}

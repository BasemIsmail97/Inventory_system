

namespace Services.Implementations
{
    public class CustomerService(IUnitOfWork unitOfWork,IMapper mapper) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateCategoryAsync(CustomerDto CreateCustomerDto)
        {
            CreateCustomerDto.CreatedAt = DateTime.Now;
            var customer = _mapper.Map<Customer>(CreateCustomerDto);
          await  _unitOfWork.GetRepository<Customer>().AddAsync(customer);
           return await _unitOfWork.SaveChangesAsync();

        }

        public async Task<bool> DeleteCategoryAsync(int CustomerId)
        {
           var  customerRepo =   _unitOfWork.GetRepository<Customer>();
            var customer = await customerRepo.GetByIdAsync(CustomerId);
            if(customer is null)
            {
                return false;
            }
            customerRepo.Delete(customer);
              await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCatygoriesAsync()
        {
            var Customers = await _unitOfWork.GetRepository<Customer>().GetAllAsync();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(Customers);
            return customerDtos;
        }

        public async Task<CustomerDto> GetCategoryByIdAsync(int CustomerId)
        {
            var customer = await _unitOfWork.GetRepository<Customer>().GetByIdAsync(CustomerId);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer {CustomerId} not found");
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

        public async Task<int> UpdateCategoryAsync(CustomerDto updateCustomerDto)
        {
            var existingCustomer= await _unitOfWork.GetRepository<Customer>().GetByIdAsync(updateCustomerDto.Id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with id {updateCustomerDto.Id} not found.");
            }
            updateCustomerDto.UpdatedAt = DateTime.Now;
            var customer = _mapper.Map<Customer>(updateCustomerDto);
            _unitOfWork.GetRepository<Customer>().Update(customer);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using Domain.Contract;


namespace Services.Implementations
{
    public class SupplierService(IUnitOfWork unitOfWork ,IMapper mapper) : ISupplierServicecs
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> CreateSupplierAsync(SupplierDto createSupplierDto)
        {
           createSupplierDto.CreatedAt = DateTime.Now;
            var supplier =  _mapper.Map<Supplier>(createSupplierDto);
              await  _unitOfWork.GetRepository<Supplier>().AddAsync(supplier);
            return await _unitOfWork.SaveChangesAsync();

        }

        public async Task<bool> DeleteSupplierAsync(int SupplierId)
        {
            var SupplierRepo =   _unitOfWork.GetRepository<Supplier>();
            var supplier =  await SupplierRepo.GetByIdAsync(SupplierId);
            if(supplier is null)
            {
                return false;
            }
            SupplierRepo.Delete(supplier);
              await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var Suppliers = await  _unitOfWork.GetRepository<Supplier>().GetAllAsync();
            var supplierDtos =  _mapper.Map<IEnumerable<SupplierDto>>(Suppliers);
            return supplierDtos;
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int SupplierId)
        {
           var supplier = await _unitOfWork.GetRepository<Supplier>().GetByIdAsync(SupplierId);
            if (supplier == null)
            {
                throw new KeyNotFoundException($"Supplier {SupplierId} not found");
            }
            var supplierDto =  _mapper.Map<SupplierDto>(supplier);
            return supplierDto;
        }

        public async Task<int> UpdateSupplierAsync(SupplierDto updateSupplierDto)
        {
           var ExistingSupplier = await _unitOfWork.GetRepository<Supplier>().GetByIdAsync(updateSupplierDto.Id);
            if(ExistingSupplier is null)
            {
                throw new KeyNotFoundException($"Supplier {updateSupplierDto.Id} not found");
            }
            updateSupplierDto.UpdatedAt = DateTime.Now;
            var supplier =  _mapper.Map<Supplier>(updateSupplierDto);
            _unitOfWork.GetRepository<Supplier>().Update(supplier);
            return  await _unitOfWork.SaveChangesAsync();
        }
    }
}

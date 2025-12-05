

namespace Services.Implementations
{
    public class PurchaseOrderService(IUnitOfWork unitOfWork ,IMapper mapper) : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public Task<int> CreatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderDto createPurchaseOrderDto)
        {
            createPurchaseOrderDto.CreatedAt = DateTime.Now;
            var purchaseOrder = _mapper.Map<PurchaseOrder>(createPurchaseOrderDto);
            _unitOfWork.GetRepository<PurchaseOrder>().AddAsync(purchaseOrder);
            return _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeletePurchaseOrderAsync(int PurchaseOrderId)
        {
            var Repo =   _unitOfWork.GetRepository<PurchaseOrder>();
            var purchaseOrder = await Repo.GetByIdAsync(PurchaseOrderId);
            if(purchaseOrder is null)
            {
                return false;
            }
            Repo.Delete(purchaseOrder);
            await  _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagenatedResult<PurchaseOrderDto>> GetAllPurchaseOrderAsync(PurchaseOrderSpecificationParameters specificationParameters)
        {
            var Repo = _unitOfWork.GetRepository<PurchaseOrder>();
            var specification = new PurchaseOrderWithSupplierAndUserAndPurchaseOrdersDetailsSpecification(specificationParameters);
            var PurchaseOrders = await Repo .GetAllAsync(specification);
            var purchaseOrderDtos = _mapper.Map<IEnumerable<PurchaseOrderDto>>(PurchaseOrders);
            var pageSize = purchaseOrderDtos.Count();
            var countSpecification = new PurchaseOrderCountSpecification(specificationParameters);
            var TotalCount = await Repo.CountAsync(countSpecification);
            return new PagenatedResult<PurchaseOrderDto>(specificationParameters.PageIndex, pageSize, TotalCount, purchaseOrderDtos);

        }

        public async Task<PurchaseDetailsDto> GetPurchaseOrderByIdAsync(int PurchaseOrderId)
        {
            var Spec = new PurchaseOrderWithSupplierAndUserAndPurchaseOrdersDetailsSpecification(PurchaseOrderId);
            var purchaseOrder = await _unitOfWork.GetRepository<PurchaseOrder>().GetByIdAsync(Spec);
            if (purchaseOrder is null)
            {
                throw new KeyNotFoundException($"Purchase Order with id {PurchaseOrderId} not found.");
            }
            var purchaseOrderDto = _mapper.Map<PurchaseDetailsDto>(purchaseOrder);
            return purchaseOrderDto;

        }

        public async Task<int> UpdatePurchaseOrderAsync(CreateOrUpdatePurchaseOrderDto updatePurchaseOrderDto)
        {
            var existingPurchaseOrder = await  _unitOfWork.GetRepository<PurchaseOrder>().GetByIdAsync(updatePurchaseOrderDto.Id);
            if(existingPurchaseOrder is null)
            {
                throw new KeyNotFoundException($"Purchase Order with id {updatePurchaseOrderDto.Id} not found.");
            }
            updatePurchaseOrderDto.UpdatedAt = DateTime.Now;
            var updatedPurchaseOrder =_mapper.Map<PurchaseOrder>(updatePurchaseOrderDto);
            _unitOfWork.GetRepository<PurchaseOrder>().Update(updatedPurchaseOrder);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}

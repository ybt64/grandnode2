﻿using Grand.Api.DTOs.Catalog;
using Grand.Api.DTOs.Common;
using Grand.Api.DTOs.Customers;
using Grand.Api.DTOs.Shipping;
using Grand.Api.Infrastructure.DependencyManagement;
using Grand.Domain.Catalog;
using Grand.Infrastructure.Configuration;
using Microsoft.OData.ModelBuilder;

namespace Grand.Api.Infrastructure
{
    public class DependencyEdmModel : IDependencyEdmModel
    {
        protected void RegisterCommon(ODataConventionModelBuilder builder)
        {
            #region Language model

            builder.EntitySet<LanguageDto>("Language");

            #endregion

            #region Currency model

            builder.EntitySet<CurrencyDto>("Currency");

            #endregion

            #region Store model

            builder.EntitySet<StoreDto>("Store");
            
            #endregion

            #region Country model

            builder.EntitySet<CountryDto>("Country");

            #endregion

            #region State province model

            builder.EntitySet<StateProvinceDto>("StateProvince");
            
            #endregion

            #region layout model

            builder.EntitySet<LayoutDto>("CategoryLayout");
            builder.EntitySet<LayoutDto>("CollectionLayout");
            builder.EntitySet<LayoutDto>("ProductLayout");
            builder.EntitySet<LayoutDto>("BrandLayout");

            #endregion

            #region Picture model

            builder.EntitySet<PictureDto>("Picture");

            #endregion
        }
        protected void RegisterProduct(ODataConventionModelBuilder builder)
        {
            builder.EntitySet<ProductDto>("Product");
            var product = builder.EntityType<ProductDto>();
            builder.ComplexType<ProductCategoryDto>();
            builder.ComplexType<ProductCollectionDto>();
            builder.ComplexType<ProductPictureDto>();
            builder.ComplexType<ProductSpecificationAttributeDto>();
            builder.ComplexType<ProductTierPriceDto>();
            builder.ComplexType<ProductWarehouseInventoryDto>();
            builder.ComplexType<ProductAttributeMappingDto>();
            builder.ComplexType<ProductAttributeValueDto>();
            builder.ComplexType<ProductAttributeCombinationDto>();
            builder.ComplexType<ProductUpdateStock>();
            builder.ComplexType<ProductCategoryDeleteDto>();
            builder.ComplexType<ProductCollectionDeleteDto>();
            builder.ComplexType<ProductPictureDeleteDto>();
            builder.ComplexType<ProductSpecificationAttributeDeleteDto>();
            builder.ComplexType<ProductTierPriceDeleteDto>();
            builder.ComplexType<ProductAttributeMappingDeleteDto>();

            //update stock for product
            ActionConfiguration updateStock = product.Action("UpdateStock");
            updateStock.Parameter<string>("WarehouseId");
            updateStock.Parameter<int>("Stock").Required();
            updateStock.Returns<bool>();

            //insert/update/delete category
            #region Product category
            ActionConfiguration createCategory = product.Action("CreateProductCategory");
            createCategory.Parameter<string>(nameof(ProductCategoryDto.CategoryId)).Required();
            createCategory.Parameter<bool>(nameof(ProductCategoryDto.IsFeaturedProduct));
            createCategory.Returns<bool>();

            ActionConfiguration updateCategory = product.Action("UpdateProductCategory");
            updateCategory.Parameter<string>(nameof(ProductCategoryDto.CategoryId)).Required();
            updateCategory.Parameter<bool>(nameof(ProductCategoryDto.IsFeaturedProduct));
            updateCategory.Returns<bool>();

            ActionConfiguration deleteCategory = product.Action("DeleteProductCategory");
            deleteCategory.Parameter<string>(nameof(ProductCategoryDto.CategoryId)).Required();
            deleteCategory.Returns<bool>();
            #endregion

            //insert/update/delete collection
            #region Product collection
            ActionConfiguration createCollection = product.Action("CreateProductCollection");
            createCollection.Parameter<string>(nameof(ProductCollectionDto.CollectionId)).Required();
            createCollection.Parameter<bool>(nameof(ProductCollectionDto.IsFeaturedProduct));
            createCollection.Returns<bool>();

            ActionConfiguration updateCollection = product.Action("UpdateProductCollection");
            updateCollection.Parameter<string>(nameof(ProductCollectionDto.CollectionId)).Required();
            updateCollection.Parameter<bool>(nameof(ProductCollectionDto.IsFeaturedProduct));
            updateCollection.Returns<bool>();

            ActionConfiguration deleteCollection = product.Action("DeleteProductCollection");
            deleteCollection.Parameter<string>(nameof(ProductCollectionDto.CollectionId)).Required();
            deleteCollection.Returns<bool>();
            #endregion

            //insert/update/delete picture
            #region Product picture
            ActionConfiguration createPicture = product.Action("CreateProductPicture");
            createPicture.Parameter<string>(nameof(ProductPictureDto.PictureId)).Required();
            createPicture.Parameter<int>(nameof(ProductPictureDto.DisplayOrder)).Required();
            createPicture.Returns<bool>();

            ActionConfiguration updatePicture = product.Action("UpdateProductPicture");
            updatePicture.Parameter<string>(nameof(ProductPictureDto.PictureId)).Required();
            updatePicture.Parameter<int>(nameof(ProductPictureDto.DisplayOrder)).Required();
            updatePicture.Returns<bool>();

            ActionConfiguration deletePicture = product.Action("DeleteProductPicture");
            deletePicture.Parameter<string>(nameof(ProductPictureDto.PictureId)).Required();
            deletePicture.Returns<bool>();
            #endregion

            #region Product specification
            ActionConfiguration createSpecification = product.Action("CreateProductSpecification");
            createSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.Id));
            createSpecification.Parameter<int>(nameof(ProductSpecificationAttributeDto.DisplayOrder));
            createSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.CustomValue));
            //createSpecification.Parameter<SpecificationAttributeType>(nameof(ProductSpecificationAttributeDto.AttributeType)).Required();
            createSpecification.Parameter<bool>(nameof(ProductSpecificationAttributeDto.AllowFiltering));
            createSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.SpecificationAttributeId));
            createSpecification.Parameter<bool>(nameof(ProductSpecificationAttributeDto.ShowOnProductPage));
            createSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.SpecificationAttributeOptionId));

            createSpecification.Returns<bool>();

            ActionConfiguration updateSpecification = product.Action("UpdateProductSpecification");
            updateSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.Id)).Required();
            updateSpecification.Parameter<int>(nameof(ProductSpecificationAttributeDto.DisplayOrder));
            updateSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.CustomValue));
            //updateSpecification.Parameter<SpecificationAttributeType>(nameof(ProductSpecificationAttributeDto.AttributeType)).Required();
            updateSpecification.Parameter<bool>(nameof(ProductSpecificationAttributeDto.AllowFiltering));
            updateSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.SpecificationAttributeId)).Required();
            updateSpecification.Parameter<bool>(nameof(ProductSpecificationAttributeDto.ShowOnProductPage));
            updateSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.SpecificationAttributeOptionId));
            updateSpecification.Returns<bool>();

            ActionConfiguration deleteSpecification = product.Action("DeleteProductSpecification");
            deleteSpecification.Parameter<string>(nameof(ProductSpecificationAttributeDto.Id)).Required();
            deleteSpecification.Returns<bool>();
            #endregion

            #region Product attribute mapping

            ActionConfiguration createProductAttributeMapping = product.Action("CreateProductAttributeMapping");
            createProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.Id));
            createProductAttributeMapping.Parameter<int>(nameof(ProductAttributeMappingDto.DisplayOrder));
            createProductAttributeMapping.Parameter<AttributeControlType>(nameof(ProductAttributeMappingDto.AttributeControlTypeId)).Required();
            createProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.DefaultValue));
            createProductAttributeMapping.Parameter<bool>(nameof(ProductAttributeMappingDto.IsRequired));
            createProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.TextPrompt));
            createProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.ValidationFileAllowedExtensions));
            createProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationFileMaximumSize));
            createProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationMaxLength));
            createProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationMinLength));
            createProductAttributeMapping.Parameter<List<ProductAttributeValueDto>>(nameof(ProductAttributeMappingDto.ProductAttributeValues));
            createProductAttributeMapping.Returns<ProductAttributeMappingDto>();

            ActionConfiguration updateProductAttributeMapping = product.Action("UpdateProductAttributeMapping");
            updateProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.Id)).Required();
            updateProductAttributeMapping.Parameter<int>(nameof(ProductAttributeMappingDto.DisplayOrder));
            updateProductAttributeMapping.Parameter<AttributeControlType>(nameof(ProductAttributeMappingDto.AttributeControlTypeId)).Required();
            updateProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.DefaultValue));
            updateProductAttributeMapping.Parameter<bool>(nameof(ProductAttributeMappingDto.IsRequired));
            updateProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.TextPrompt));
            updateProductAttributeMapping.Parameter<string>(nameof(ProductAttributeMappingDto.ValidationFileAllowedExtensions));
            updateProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationFileMaximumSize));
            updateProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationMaxLength));
            updateProductAttributeMapping.Parameter<int?>(nameof(ProductAttributeMappingDto.ValidationMinLength));
            updateProductAttributeMapping.Parameter<List<ProductAttributeValueDto>>(nameof(ProductAttributeMappingDto.ProductAttributeValues));
            updateProductAttributeMapping.Returns<ProductAttributeMappingDto>();

            ActionConfiguration deleteProductAttributeMapping = product.Action("DeleteProductAttributeMapping");
            deleteProductAttributeMapping.Parameter<string>(nameof(ProductSpecificationAttributeDto.Id)).Required();
            deleteProductAttributeMapping.Returns<bool>();

            #endregion

            //insert/update/delete tier price
            #region Product tierprice

            ActionConfiguration createTierPrice = product.Action("CreateProductTierPrice");
            createTierPrice.Parameter<int>(nameof(ProductTierPriceDto.Quantity));
            createTierPrice.Parameter<double>(nameof(ProductTierPriceDto.Price));
            createTierPrice.Parameter<string>(nameof(ProductTierPriceDto.StoreId));
            createTierPrice.Parameter<string>(nameof(ProductTierPriceDto.CustomerGroupId));
            createTierPrice.Parameter<DateTime?>(nameof(ProductTierPriceDto.StartDateTimeUtc));
            createTierPrice.Parameter<DateTime?>(nameof(ProductTierPriceDto.EndDateTimeUtc));
            createTierPrice.Returns<bool>();

            ActionConfiguration updateTierPrice = product.Action("UpdateProductTierPrice");
            updateTierPrice.Parameter<string>(nameof(ProductTierPriceDto.Id)).Required();
            updateTierPrice.Parameter<int>(nameof(ProductTierPriceDto.Quantity));
            updateTierPrice.Parameter<double>(nameof(ProductTierPriceDto.Price));
            updateTierPrice.Parameter<string>(nameof(ProductTierPriceDto.StoreId));
            updateTierPrice.Parameter<string>(nameof(ProductTierPriceDto.CustomerGroupId));
            updateTierPrice.Parameter<DateTime?>(nameof(ProductTierPriceDto.StartDateTimeUtc));
            updateTierPrice.Parameter<DateTime?>(nameof(ProductTierPriceDto.EndDateTimeUtc));
            updateTierPrice.Returns<bool>();

            ActionConfiguration deleteTierPrice = product.Action("DeleteProductTierPrice");
            deleteTierPrice.Parameter<string>(nameof(ProductTierPriceDto.Id)).Required();
            deleteTierPrice.Returns<bool>();

            #endregion
        }

        protected void RegisterCatalog(ODataConventionModelBuilder builder)
        {
            #region Category model

            builder.EntitySet<CategoryDto>("Category");

            #endregion

            #region Brand model

            builder.EntitySet<BrandDto>("Brand");

            #endregion

            #region Collection model

            builder.EntitySet<CollectionDto>("Collection");
            #endregion

            #region Product attribute model

            builder.EntitySet<ProductAttributeDto>("ProductAttribute");
            builder.ComplexType<PredefinedProductAttributeValueDto>();

            #endregion

            #region Product attribute model

            builder.EntitySet<SpecificationAttributeDto>("SpecificationAttribute");
            builder.ComplexType<SpecificationAttributeOptionDto>();

            #endregion

        }

        protected void RegisterCustomers(ODataConventionModelBuilder builder)
        {
            #region Customer

            builder.EntitySet<CustomerDto>("Customer");
            var customer = builder.EntityType<CustomerDto>();
            builder.ComplexType<AddressDto>();
            builder.ComplexType<DeleteAddressDto>();
            builder.ComplexType<PasswordDto>();

            ActionConfiguration addAddress = customer.Action("AddAddress");
            addAddress.Parameter<string>(nameof(AddressDto.Id)).Required();
            addAddress.Parameter<string>(nameof(AddressDto.City));
            addAddress.Parameter<string>(nameof(AddressDto.Email));
            addAddress.Parameter<string>(nameof(AddressDto.Company));
            addAddress.Parameter<string>(nameof(AddressDto.Address1));
            addAddress.Parameter<string>(nameof(AddressDto.Address2));
            addAddress.Parameter<string>(nameof(AddressDto.LastName));
            addAddress.Parameter<string>(nameof(AddressDto.CountryId));
            addAddress.Parameter<string>(nameof(AddressDto.FaxNumber));
            addAddress.Parameter<string>(nameof(AddressDto.FirstName));
            addAddress.Parameter<string>(nameof(AddressDto.VatNumber));
            addAddress.Parameter<string>(nameof(AddressDto.PhoneNumber));
            addAddress.Parameter<string>(nameof(AddressDto.Note));
            addAddress.Parameter<int>(nameof(AddressDto.AddressType));
            addAddress.Parameter<DateTimeOffset>(nameof(AddressDto.CreatedOnUtc));
            addAddress.Parameter<string>(nameof(AddressDto.ZipPostalCode));
            addAddress.Parameter<string>(nameof(AddressDto.StateProvinceId));
            addAddress.Returns<AddressDto>();

            ActionConfiguration updateAddress = customer.Action("UpdateAddress");
            updateAddress.Parameter<string>(nameof(AddressDto.Id)).Required();
            updateAddress.Parameter<string>(nameof(AddressDto.City));
            updateAddress.Parameter<string>(nameof(AddressDto.Email));
            updateAddress.Parameter<string>(nameof(AddressDto.Company));
            updateAddress.Parameter<string>(nameof(AddressDto.Address1));
            updateAddress.Parameter<string>(nameof(AddressDto.Address2));
            updateAddress.Parameter<string>(nameof(AddressDto.LastName));
            updateAddress.Parameter<string>(nameof(AddressDto.CountryId));
            updateAddress.Parameter<string>(nameof(AddressDto.FaxNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.FirstName));
            updateAddress.Parameter<string>(nameof(AddressDto.VatNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.PhoneNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.Note));
            updateAddress.Parameter<int>(nameof(AddressDto.AddressType));
            updateAddress.Parameter<DateTimeOffset>(nameof(AddressDto.CreatedOnUtc));
            updateAddress.Parameter<string>(nameof(AddressDto.ZipPostalCode));
            updateAddress.Parameter<string>(nameof(AddressDto.StateProvinceId));
            updateAddress.Returns<AddressDto>();

            ActionConfiguration deleteAddress = customer.Action("DeleteAddress");
            deleteAddress.Parameter<string>(nameof(DeleteAddressDto.AddressId));
            deleteAddress.Returns<bool>();

            ActionConfiguration changePassword = customer.Action("SetPassword");
            changePassword.Parameter<string>(nameof(PasswordDto.Password));
            changePassword.Returns<bool>();

            #endregion

            #region Customer group model

            builder.EntitySet<CustomerGroupDto>("CustomerGroup");

            #endregion

            #region Vendors

            builder.EntitySet<VendorDto>("Vendor");
            
            #endregion
        }

        protected void RegisterShipping(ODataConventionModelBuilder builder)
        {
            #region Warehouse model

            builder.EntitySet<WarehouseDto>("Warehouse");
            
            #endregion

            #region Delivery date model

            builder.EntitySet<DeliveryDateDto>("DeliveryDate");
            
            #endregion

            #region Pickup point model

            builder.EntitySet<PickupPointDto>("PickupPoint");

            #endregion

            #region Shipping method model

            builder.EntitySet<ShippingMethodDto>("ShippingMethod");

            #endregion
        }

        public void Register(ODataConventionModelBuilder builder, ApiConfig apiConfig)
        {
            if (apiConfig.SystemModel)
            {
                RegisterCommon(builder);
                RegisterProduct(builder);
                RegisterCatalog(builder);
                RegisterCustomers(builder);
                RegisterShipping(builder);
            }
        }

        public int Order => 0;
    }
}

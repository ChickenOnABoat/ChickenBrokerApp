CreateAsync
- Should return address, When Address is Created
- Should return null, When Address is not Created
- Should throw exception with Message "Address not found", When IBrasilAPI.CEP_V2 returns null
- Should throw exception with Message "UF not available on Address", When IBrasilAPI.IBGE_UF returns null

GetByIdAsync
- Should return address specified by Id, When Address exist
- Should return null, When Address does not exist

GetByZipCodeAsync
- Should return address specified by ZipCode, When Address exist
- Should return null, When Address does not exist

GetByZipCodeAndCreateIfNotExistsAsync
- Should return address, When Address exist
- Should create and return address, When Address does not exist
- Should return null, When Address is not Created
- Should throw exception , When IBrasilAPI.CEP_V2 returns null
- Should throw exception with Message "UF not available on Address", When IBrasilAPI.IBGE_UF returns null

GetAllAsync
- Should return list of Addresses, When Adresses exist
- Should return empty list, When No Address exist

UpdateAsync
- Should return Address, When Address is Updated
- Should return null, When Address is not updated

DeleteByIdAsync
- Should return true, When Address is Deleted
- Should return false, When Address is not Deleted

ExistsByIdAsync
- Should return true, When Address exist
- Should return false, When Address not exist

ExistsByZipCodeAsync
- Should return true, When Address exist
- Should return false, When Address not exist



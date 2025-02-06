CreateAsync
- Should return null, When Address is null
- Should return PropertyAgency, When PropertyAgency is Created
- Should return null, When PropertyAgency is not Created

GetByIdAsync
- Should return PropertyAgency, When PropertyAgency exists
- Should return null, When PropertyAgency does not exist

GetAllAsync
- Should return List of PropertyAgencies, When PropertyAgency exist
- Should return empty list, When no PropertyAgency exist

GetCountAllAsync
- Should return quantity of existing addresses

UpdateAsync
- Should return null, When Address is null
- Should return PropertyAgency, When PropertyAgency is Updated
- Should return null, When PropertyAgency is not Updated

DeleteByIdAsync
- Should return True, When PropertyAgency is Deleted
- Should return True, When PropertyAgency is not Deleted

ExistsByIdAsync
- Should return true, When PropertyAgency exists
- Should return False, When PropertyAgency does not exist
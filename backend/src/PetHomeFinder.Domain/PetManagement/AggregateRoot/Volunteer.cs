using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.AggregateRoot
{
    public class Volunteer : Shared.Entity<VolunteerId>
    {
        private bool _isDeleted = false;

        private readonly List<Pet> _petsOwning = [];
        private List<Credential> _credentials = [];
        private List<SocialNetwork> _socialNetworks = [];

        public Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(
            VolunteerId id,
            FullName fullName,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber,
            IEnumerable<Credential> credentials,
            IEnumerable<SocialNetwork> socialNetworks) : base(id)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
            _credentials = credentials.ToList();
            _socialNetworks = socialNetworks.ToList();
        }

        public FullName FullName { get; private set; }

        public Description Description { get; private set; }

        public Experience Experience { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; }

        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

        public IReadOnlyList<Credential> Credentials => _credentials;

        public IReadOnlyList<Pet> PetsOwning => _petsOwning;

        public int GetPetHomeFoundCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.FOUND_HOME).Count();

        public int GetPetSearchForHomeCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.SEARCH_FOR_HOME).Count();

        public int GetPetInTreatmentCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.NEED_TREATMENT).Count();

        public void UpdateMainInfo(
            FullName fullName,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
        }

        public void UpdateCredentials(IEnumerable<Credential> credentials)
        {
            _credentials = credentials.ToList();
        }

        public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
        {
            _socialNetworks = socialNetworks.ToList();
        }

        public void SoftDelete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
            foreach (var pet in _petsOwning)
                pet.SoftDelete();
        }

        public UnitResult<Error> AddPet(Pet pet)
        {
            var positionResult = Position.Create(PetsOwning.Count + 1);
            if (positionResult.IsFailure)
                return positionResult.Error;

            pet.SetPosition(positionResult.Value);

            _petsOwning.Add(pet);

            return Result.Success<Error>();
        }
        
        public void UpdatePet(
            PetId petId, 
            Name name,
            Description description,
            SpeciesBreed speciesBreed,
            Color color,
            HealthInfo healthInfo,
            Address address,
            Weight weight,
            Height height,
            PhoneNumber phoneNumber,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthDate,
            IEnumerable<Credential> credentials)
        {
            _petsOwning.FirstOrDefault(p => p.Id == petId)?.Update(
                name,
                description,
                speciesBreed,
                color,
                healthInfo,
                address,
                weight,
                height,
                phoneNumber,
                isCastrated,
                isVaccinated,
                birthDate,
                credentials);
        }

        public Result<Pet, Error> GetPetById(PetId petId)
        {
            var pet = PetsOwning.FirstOrDefault(p => p.Id == petId);
            if (pet is null)
                return Errors.General.NotFound(petId.Value);

            return pet;
        }

        public UnitResult<Error> MovePet(Pet pet, Position newPosition)
        {
            var currentPosition = pet.Position;
            if (currentPosition == newPosition || PetsOwning.Count == 1)
                return Result.Success<Error>();

            var adjustedPosition = AdjustPositionIfOutOfRange(newPosition);
            if (adjustedPosition.IsFailure)
                return adjustedPosition.Error;

            newPosition = adjustedPosition.Value;

            var result = MovePetsBetweenPositions(newPosition, currentPosition);
            if (result.IsFailure)
                return result.Error;

            pet.SetPosition(newPosition);

            return Result.Success<Error>();
        }

        private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
        {
            if (newPosition < currentPosition)
            {
                var petsToMove = PetsOwning.Where(p =>
                    p.Position >= newPosition
                    && p.Position < currentPosition);

                foreach (var petToMove in petsToMove)
                {
                    var result = petToMove.MoveForward();
                    if (result.IsFailure)
                        return result.Error;
                }
            }
            else if (newPosition > currentPosition)
            {
                var petsToMove = PetsOwning.Where(p =>
                    p.Position <= newPosition
                    && p.Position > currentPosition);

                foreach (var petToMove in petsToMove)
                {
                    var result = petToMove.MoveBack();
                    if (result.IsFailure)
                        return result.Error;
                }
            }

            return Result.Success<Error>();
        }

        private Result<Position, Error> AdjustPositionIfOutOfRange(Position newPosition)
        {
            if (newPosition.Value <= PetsOwning.Count)
                return newPosition;

            var lastPosition = Position.Create(PetsOwning.Count);
            if (lastPosition.IsFailure)
                return lastPosition.Error;

            return lastPosition.Value;
        }
    }
}
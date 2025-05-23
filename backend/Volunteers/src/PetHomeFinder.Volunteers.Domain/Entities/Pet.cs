﻿using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.ValueObjects;

namespace PetHomeFinder.Volunteers.Domain.Entities
{
    public class Pet : SharedKernel.Entity<PetId>
    {
        private bool _isDeleted = false;
        private List<Credential> _credentials = [];
        private List<PetPhoto> _photos = [];

        public Pet(PetId id) : base(id)
        {
        }

        public Pet(
            PetId id,
            Name name,
            SpeciesBreed speciesBreed,
            Description description,
            Color color,
            HealthInfo healthInfo,
            Address address,
            Weight weight,
            Height height,
            PhoneNumber ownerPhoneNumber,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthDate,
            HelpStatusEnum helpStatus,
            IEnumerable<Credential> credentials,
            DateTime createDate,
            IEnumerable<PetPhoto>? photos = null) : base(id)
        {
            Name = name;
            SpeciesBreed = speciesBreed;
            Description = description;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhoneNumber = ownerPhoneNumber;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            BirthDate = birthDate;
            HelpStatus = helpStatus;
            CreateDate = createDate;

            _credentials = credentials.ToList();
            
            _photos = photos == null
                ? Enumerable.Empty<PetPhoto>().ToList()
                : photos.ToList();
        }

        public Name Name { get; private set; }

        public SpeciesBreed SpeciesBreed { get; private set; }

        public Position Position { get; private set; }

        public Description Description { get; private set; }

        public Color Color { get; private set; }

        public HealthInfo HealthInfo { get; private set; }

        public Address Address { get; private set; }

        public Weight Weight { get; private set; }

        public Height Height { get; private set; }

        public PhoneNumber OwnerPhoneNumber { get; private set; }

        public bool IsCastrated { get; private set; }

        public bool IsVaccinated { get; private set; }

        public DateTime BirthDate { get; private set; }

        public HelpStatusEnum HelpStatus { get; private set; }

        public IReadOnlyList<Credential> Credentials => _credentials;

        public DateTime CreateDate { get; private set; }

        public IReadOnlyList<PetPhoto> Photos => _photos;

        public void SoftDelete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
        }

        public void UpdatePhotos(IEnumerable<PetPhoto> photos)
        {
            _photos = photos.ToList();
        }

        public void SetPosition(Position position) =>
            Position = position;

        public UnitResult<Error> MoveForward()
        {
            var newPosition = Position.Forward();
            if (newPosition.IsFailure)
                return newPosition.Error;

            Position = newPosition.Value;

            return Result.Success<Error>();
        }

        public UnitResult<Error> MoveBack()
        {
            var newPosition = Position.Back();
            if (newPosition.IsFailure)
                return newPosition.Error;

            Position = newPosition.Value;

            return Result.Success<Error>();
        }

        public void Update(
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
            Name = name;
            Description = description;
            SpeciesBreed = speciesBreed;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhoneNumber = phoneNumber;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            BirthDate = birthDate;
            _credentials = credentials.ToList();
        }

        public void UpdateStatus(HelpStatusEnum status)
        {
            HelpStatus = status;
        }

        public UnitResult<Error> UpdateMainPhoto(PetPhoto newMainPhoto)
        {
            var photoExists = _photos.FirstOrDefault(p => p.FilePath == newMainPhoto.FilePath);
            if (photoExists is null)
                return Errors.General.NotFound();

            _photos = _photos
                .Select(p => 
                    PetPhoto.Create(p.FilePath, p.FilePath == newMainPhoto.FilePath).Value)
                .OrderByDescending(p => p.IsMain)
                .ToList();

            return Result.Success<Error>();
        }
    }

    public enum HelpStatusEnum
    {
        NEED_TREATMENT,
        SEARCH_FOR_HOME,
        FOUND_HOME
    }
}
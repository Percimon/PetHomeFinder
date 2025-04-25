using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Domain.ValueObjects;

public class Position
{
    public static Position First = new(1);

    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward() => Create(Value + 1);

    public Result<Position, Error> Back() => Create(Value - 1);

    public static Result<Position, Error> Create(int number)
    {
        if (number < 1)
            return Errors.General.ValueIsInvalid("serial number");

        return new Position(number);
    }

    public static implicit operator int(Position position) => position.Value;

    public static implicit operator Position(int value) => Create(value).Value;
}
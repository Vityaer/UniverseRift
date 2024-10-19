using Models.Heroes.HeroCharacteristics.Abstractions;

namespace Models.Heroes.HeroCharacteristics
{
    public class CharacteristicContainer<T> : AbstractCharacteristicContainer
    {
        public T Value;
    }
}

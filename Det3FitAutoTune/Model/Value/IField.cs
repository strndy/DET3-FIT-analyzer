using System;
namespace Det3FitAutoTune.Model.Value
{
    public interface IField<T> : IField
    {
        T Bytes { get; set; }
    }

    public interface IField
    {
        float Value { get; set; }
    }
}

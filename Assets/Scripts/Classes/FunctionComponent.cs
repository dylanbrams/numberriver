using System;

public abstract class FunctionComponent
{
	public abstract FunctionComponentType GetFunctionComponentType();

	public abstract void MultiplyComponent();

	public abstract void DeriveComponent();

	public abstract void IntegrateComponent();

	public abstract void AddToComponent(float cofactorInput);

	public abstract void GenerateRandomComponent();

	public abstract void SetLatexRepresentation();

	public abstract string GETLatexRepresentation();

	public abstract double GETCurrentValue(double t, int p);

}

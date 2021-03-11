using System;

public abstract class FunctionComponent
{
	public abstract FunctionComponentType GetFunctionComponentType();

	public abstract void MultiplyComponent();

	public abstract void DeriveComponent();

	public abstract void IntegrateComponent();

	public abstract void AddToComponent(float cofactorInput);

	public abstract void generateRandomComponent();

	public abstract void setLatexRepresentation();

	public abstract string getLatexRepresentation();

	public abstract double getCurrentValue(double t, int p);

}

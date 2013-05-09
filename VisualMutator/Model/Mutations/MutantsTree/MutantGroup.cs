namespace VisualMutator.Model.Mutations.MutantsTree
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonUtilityInfrastructure;
    using VisualMutator.Extensibility;

    public class MutantGroup : MutationNode
    {
       

        public MutantGroup(string name)
            : base( name, true)
        {
           
        }

        public IEnumerable<Mutant> Mutants
        {
            get
            {
                return Children.Cast<Mutant>();
            }
        }


        private string _displayedText;

     

        public string DisplayedText
        {
            get
            {
                return _displayedText;
            }
            set
            {
                SetAndRise(ref _displayedText, value, () => DisplayedText);
            }
        }

        

        public void UpdateDisplayedText()
        {
            DisplayedText = "Mutation group: {1} - Mutants: {2}"
                    .Formatted(Name, Children.Count);
        }
    }
}
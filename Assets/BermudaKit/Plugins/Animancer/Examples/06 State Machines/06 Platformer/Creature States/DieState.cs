// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.StateMachines.Platformer
{
    /// <summary>
    /// A <see cref="CreatureState"/> that plays a die animation.
    /// </summary>
    [AddComponentMenu(Strings.MenuPrefix + "Examples/Platformer - Die State")]
    [HelpURL(Strings.APIDocumentationURL + ".Examples.StateMachines.Platformer/DieState")]
    public sealed class DieState : CreatureState
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void Awake()
        {
            Creature.Health.OnHealthChanged += () =>
            {
                if (Creature.Health.CurrentHealth <= 0)
                {
                    Creature.StateMachine.ForceSetState(this);
                    var state = Creature.Animancer.States[_Animation];
                    state.Speed = 1;
                }
                else if (enabled)
                {
                    var state = Creature.Animancer.States[_Animation];
                    state.Speed = -1;
                    if (state.NormalizedTime > 1)
                        state.NormalizedTime = 1;
                    state.Events.OnEnd = Creature.ForceEnterIdleState;
                }
            };
        }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/

        public override bool CanExitState(CreatureState nextState)
        {
            return false;
        }

        /************************************************************************************************************************/
    }
}

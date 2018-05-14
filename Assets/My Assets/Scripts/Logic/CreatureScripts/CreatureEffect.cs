using UnityEngine;
using System.Collections;

public abstract class CreatureEffect 
{
    protected Player owner;
    protected CreatureLogic creature;
    protected int specialAmount;

    public CreatureEffect(Player owner, CreatureLogic creature, int specialAmount)
    {
        this.creature = creature;
        this.owner = owner;
        this.specialAmount = specialAmount;
    }

	// METHODS FOR SPECIAL FX THAT LISTEN TO EVENTS
    public abstract void RegisterEventEffect();

	public abstract void UnRegisterEventEffect ();

    public abstract void CauseEventEffect();

    // BATTLECRY
    public virtual void WhenACreatureIsPlayed(){}

    // DEATHRATTLE
    public virtual void WhenACreatureDies(){}

}

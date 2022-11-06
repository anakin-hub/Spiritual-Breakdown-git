using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject AttackButton;
    public GameObject SkillButton;
    public GameObject EscapeButton;

    public BattleState state;
    
    protected Unit playerUnit;
    protected Unit enemyUnit;

    public TMP_Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject _finalScene;

    protected bool endgame;

    void Start()
    {
        endgame = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    protected void Update()
    {
        if (endgame)
            if (Input.anyKeyDown)
                Application.Quit();
    }

    IEnumerator SetupBattle()
    {
        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();

        dialogueText.text = "Um " + enemyUnit.UnitName + " selvagem aparece...";

        //playerHUD.SetHUD(playerUnit);
        //enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        enableButtons();
        dialogueText.text = "Escolha sua ação: ";
    }

    void disableButtons()
    {
        AttackButton.SetActive(false);
        SkillButton.SetActive(false);
        EscapeButton.SetActive(false);
    }

    void enableButtons()
    {
        AttackButton.SetActive(true);
        SkillButton.SetActive(true);  
        EscapeButton.SetActive(true);
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.setHP(enemyUnit.currentHP);
        dialogueText.text = "O ataque foi um sucesso!";

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
	    {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
	}

	IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.UnitName + " atacou!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.setHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {   
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }


    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Você venceu o combate!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Você perdeu...";
        }

        StartCoroutine(Finishing_Game());
    }

    IEnumerator Finishing_Game()
    {
        yield return new WaitForSeconds(3f);

        _finalScene.SetActive(true);

        yield return new WaitForSeconds(2f);
        endgame = true;
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(PlayerAttack());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(10);

        playerHUD.setHP(playerUnit.currentHP);
        dialogueText.text = "Você sente sua vitalidade aumentando";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnHealButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(PlayerHeal());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject AttackButton;
    public GameObject HealButton;
    public GameObject SkillButton;
    public GameObject EscapeButton;

    protected int playerTurn;
    protected int enemyTurn;

    public BattleState state;
    
    protected Player playerUnit;
    protected Enemy enemyUnit;

    public TMP_Text TurnNameText;
    public TMP_Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject _victoryScene;
    public GameObject _defeatScene;
    public GameObject _escapeScene;

    [SerializeField] protected bool endgame;
    [SerializeField] protected bool endTurn;

    void Start()
    {
        endTurn = false;
        endgame = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    protected void Update()
    {
        if (endTurn)
        {
            string nameTurn = "Turno de ";
            dialogueText.text = "";
            if (state == BattleState.PLAYERTURN && playerUnit.state == TURNSTATE.WAITING)
            {
                endTurn = false;
                nameTurn += playerUnit.UnitName;
                TurnNameText.text = nameTurn;
                PlayerTurn(); 
            }
            else if (state == BattleState.ENEMYTURN && enemyUnit.state == TURNSTATE.WAITING)
            {
                endTurn = false;
                nameTurn += enemyUnit.UnitName;
                TurnNameText.text = nameTurn;
                EnemyTurn(); 
            }
        }
        else
        {
            if (state == BattleState.PLAYERTURN && playerUnit.state == TURNSTATE.ACTION)
            {
                if (playerUnit.Endturn())
                {
                    if (playerUnit.Victory())
                    {
                        state = BattleState.WON;
                        EndBattle();
                    }
                    else
                    {
                        playerUnit.state = TURNSTATE.WAITING;
                        state = BattleState.ENEMYTURN;
                        endTurn = true;
                    }                        
                }
            }
            else if(state == BattleState.ENEMYTURN && enemyUnit.state == TURNSTATE.ACTION)
            {
                if (enemyUnit.Endturn())
                {
                    if (enemyUnit.Victory())
                    {
                        state = BattleState.LOST;
                        EndBattle();
                    }
                    else
                    {
                        enemyUnit.state = TURNSTATE.WAITING;
                        state = BattleState.PLAYERTURN;
                        endTurn = true;
                    }
                }
            }
        }
    }

    IEnumerator SetupBattle()
    {
        playerUnit = playerPrefab.GetComponent<Player>();
        enemyUnit = enemyPrefab.GetComponent<Enemy>();

        dialogueText.text = "Um " + enemyUnit.UnitName + " selvagem aparece...";

        TMP_Text buttonText = HealButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = playerUnit._skillCura.GetSkillName();

        buttonText = AttackButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = playerUnit._ataque.GetSkillName();

        buttonText = SkillButton.GetComponentInChildren<TMP_Text>();
        buttonText.text = playerUnit._skillDano.GetSkillName();

        yield return new WaitForSeconds(2f);

        playerUnit.state = TURNSTATE.WAITING;
        enemyUnit.state = TURNSTATE.WAITING;

        playerTurn = enemyTurn = 0;

        state = BattleState.PLAYERTURN;
        endTurn = true;
        //PlayerTurn();
    }

    void EnemyTurn()
    {
        enemyUnit.state = TURNSTATE.ACTION;
        enemyTurn++;

        if (!enemyUnit._skillCura.GetReady())
            enemyUnit._skillCura.UpdateSkill(enemyTurn);
        if (!enemyUnit._ataque.GetReady())
            enemyUnit._ataque.UpdateSkill(enemyTurn);
        if (!enemyUnit._skillDano.GetReady())
            enemyUnit._skillDano.UpdateSkill(enemyTurn);

        StartCoroutine(EnemyDecide());
    }

    IEnumerator EnemyDecide()
    {
        int skill = Random.Range(1, 10);

        yield return new WaitForSeconds(1f);

        if (skill <= 4)
            enemyUnit._ataque.UseSkill(enemyTurn, enemyUnit, playerUnit, dialogueText);
        else if (skill > 4 && skill <= 7)
        {
            if (enemyUnit._skillCura.GetReady())
                enemyUnit._skillCura.UseSkill(enemyTurn, enemyUnit, dialogueText);
            else
                enemyUnit._ataque.UseSkill(enemyTurn, enemyUnit, playerUnit, dialogueText);
        }
        else
        {
            if (enemyUnit._skillDano.GetReady())
                enemyUnit._skillDano.UseSkill(enemyTurn, enemyUnit, playerUnit, dialogueText);
            else
                enemyUnit._ataque.UseSkill(enemyTurn, enemyUnit, playerUnit, dialogueText);
        }
    }

    void PlayerTurn()
    {
        playerUnit.state = TURNSTATE.ACTION;
        playerTurn++;
        if (!playerUnit._skillCura.GetReady())
            playerUnit._skillCura.UpdateSkill(playerTurn);
        if(!playerUnit._ataque.GetReady())
            playerUnit._ataque.UpdateSkill(playerTurn);
        if (!playerUnit._skillDano.GetReady())
            playerUnit._skillDano.UpdateSkill(playerTurn);

        enableButtons();
        dialogueText.text = "Escolha sua ação: ";
    }

    void disableButtons()
    {
        EscapeButton.SetActive(false);
        if(playerUnit._ataque.GetReady())
            AttackButton.SetActive(false);
        if (playerUnit._skillDano.GetReady())
            SkillButton.SetActive(false);
        if (playerUnit._skillCura.GetReady())
            HealButton.SetActive(false);
    }

    void enableButtons()
    {
        EscapeButton.SetActive(true);
        if (playerUnit._ataque.GetReady())
            AttackButton.SetActive(true);
        if (playerUnit._skillDano.GetReady())
            SkillButton.SetActive(true);
        if (playerUnit._skillCura.GetReady())
            HealButton.SetActive(true);
    }

    bool VerifyAttack(int bonusatk, int Def)
    {
        if (state == BattleState.PLAYERTURN)
            dialogueText.text = "Seu ataque: " + bonusatk + "\n Defesa do alvo: " + Def;
        else if (state == BattleState.ENEMYTURN)
            dialogueText.text = "Ataque do Inimigo: " + bonusatk + "\n Sua defesa: " + Def;

        if (bonusatk > Def)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = false;

        if(playerUnit.state != TURNSTATE.DEAD)
        {
            bool success = VerifyAttack(playerUnit.GetAtk(), enemyUnit.GetDefesa());


            yield return new WaitForSeconds(2.5f);
            if (success)
            {
                int dmg = playerUnit.GetDano();

                if (playerUnit.Critic())
                {
                    dialogueText.text = "Você acertou o inimigo em um ponto crítico!\n";
                    dmg += playerUnit.GetDano();
                }
                else
                {
                    dialogueText.text = "O ataque foi um sucesso!\n";
                }

                yield return new WaitForSeconds(2f);

                isDead = enemyUnit.TakeDamage(dmg);

                enemyHUD.setHP(enemyUnit.GetCurrentHP());
                dialogueText.text += "Você deu " + dmg + " de dano!";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        if(isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
	    {
            state = BattleState.ENEMYTURN;
            endTurn = true;
            playerUnit.state = TURNSTATE.WAITING;
        }
	}

	IEnumerator EnemyAttack()
    {
        bool isDead = false;
        if (enemyUnit.state != TURNSTATE.DEAD)
        {
            bool success = VerifyAttack(enemyUnit.GetAtk(), playerUnit.GetDefesa());

            yield return new WaitForSeconds(2.5f);

            if (success)
            {
                int dmg = enemyUnit.GetDano();

                if (enemyUnit.Critic())
                {
                    dialogueText.text = "Você recebeu um golpe em um ponto vital!\n";
                    dmg += enemyUnit.GetDano();
                }
                else
                {
                    dialogueText.text = enemyUnit.UnitName + " atacou!\n";
                }

                yield return new WaitForSeconds(2f);

                isDead = playerUnit.TakeDamage(dmg);

                playerHUD.setHP(playerUnit.GetCurrentHP());
                dialogueText.text += "Você recebeu " + dmg + " de dano";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {   
            state = BattleState.PLAYERTURN;
            endTurn = true;
            enemyUnit.state = TURNSTATE.WAITING;
        }
    }

    void EndBattle()
    {
        GameObject finalscene;

        if (state == BattleState.WON)
        {
            dialogueText.text = "Você venceu o combate!";
            finalscene = _victoryScene;
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Você perdeu...";
            finalscene = _defeatScene;
        }
        else
        {
            dialogueText.text = "Você fugiu do combate.";
            finalscene = _escapeScene;
        }

        StartCoroutine(Finishing_Game(finalscene));
    }

    IEnumerator Finishing_Game(GameObject finalscene)
    {
        yield return new WaitForSeconds(3f);

        finalscene.SetActive(true);

        yield return new WaitForSeconds(2f);
        endgame = true;
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            //StartCoroutine(PlayerAttack());
            playerUnit._ataque.UseSkill(playerTurn, playerUnit, enemyUnit, dialogueText);
        }
    }

    public void OnHealButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            playerUnit._skillCura.UseSkill(playerTurn, playerUnit, dialogueText);
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(10);

        playerHUD.setHP(playerUnit.GetCurrentHP());
        dialogueText.text = "Você sente sua vitalidade aumentando";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        playerUnit.state = TURNSTATE.WAITING;
        endTurn = true;
    }

    public void OnSkillButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            playerUnit._skillDano.UseSkill(playerTurn, playerUnit, enemyUnit, dialogueText);
        }
    }

    IEnumerator FireBall()
    {
        bool isDead = false;

        if (playerUnit.state != TURNSTATE.DEAD)
        {
            bool success = VerifyAttack(playerUnit.GetAtk()+3, enemyUnit.GetDefesa());


            yield return new WaitForSeconds(2.5f);
            if (success)
            {
                int dmg = playerUnit.GetDano();
                dmg += playerUnit.GetDano();

                dialogueText.text = "A habilidade acertou!\n";

                yield return new WaitForSeconds(2f);

                isDead = enemyUnit.TakeDamage(dmg);

                enemyHUD.setHP(enemyUnit.GetCurrentHP());
                dialogueText.text += "Você deu " + dmg + " de dano!";
            }
            else
            {
                dialogueText.text = "O ataque falhou!";
            }

            yield return new WaitForSeconds(2f);
        }

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            endTurn = true;
            playerUnit.state = TURNSTATE.WAITING;
        }
    }


    public void OnEscapeButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            disableButtons();
            StartCoroutine(EscapeCombat());
        }
    }

    IEnumerator EscapeCombat()
    {
        dialogueText.text = "Você tenta fugir do combate.";

        yield return new WaitForSeconds(2f);

        if (playerUnit.RollDice(100) > 5)
        {
            dialogueText.text = "Você escapou do combate.";
            EndBattle();
            state = BattleState.WON;
        }
        else
        {
            dialogueText.text = "Você não conseguiu escapar e o inimigo parece estar pronto para te atacar.";
            if (playerUnit.state == TURNSTATE.ACTION)
                playerUnit.state = TURNSTATE.WAITING;
            state = BattleState.ENEMYTURN;
            endTurn = true;
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

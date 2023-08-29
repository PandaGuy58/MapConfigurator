using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BattleAreas battleAreas;

    public bool player;
    public bool enemy;

    public bool interact;
    public Interactable interactable;


    Rigidbody rb;
    public WorldObject worldObject;
    public List<CharacterController> nearbyControllers;
    public List<GameObject> nearbyControllerObjects;

    HexGrid hexGrid;
    public Animator animator;
    public CharacterRandomise charRandomise;


    public List<HexTile> currentPath;
    FindPath pathFindingScript;
    Camera cam;

    // character stats
    public int maxHealth;
    public int currentHealth;

    public bool moving;
    public bool combat;

    public int currentTurns = 0;
    public int speed;

    public int damage;


    Vector2 animationState = new Vector2(1, 1);
    string deathAnimation;
    string attackAnimation;
    string takeDamageAnimation;

    float receiveDamageTime = -1;
    float attackTime = -1;

    float timeActivate;

    public LayerMask tileTargetMask;
    public LayerMask environmentTargetMask;

    public Transform lookDirection;
    CharacterController attackTarget;
    CharacterController npcTarget;
    CharacterController playerTarget;

    Interactable interactTarget;
    float interactTime = -1;

    public float forceUI;

    public bool midAction = false;
    bool attacking = false;

    float currentVisibility = 0;
    public CharacterController characterInteract;
    public float characterCooldown = -1;
    public float interruptCooldown = -1;

    public CharacterUI ui;
    float lookTargetTime = -1;
    bool deathCompleted = false;

    public GameObject recentCombatTile;
    List<HexTile> currentCombatPath = new List<HexTile> ();
    HexTile hitTile;

    public float shaderActivate = -1;
    public float shaderDeactivate = -1;
    bool attackOutOfDistance = true;
    bool neighbouring = false;


    public bool executingTurn;
    public AnimationCurve curve;

    public float lastCombatRaycast;
    public float selectedTime;
    public float shaderTime;
    public float pointerTime;
    public float forceInvisibleTime;

    public bool moveAllowed = false;
    public bool moveComplete = false;

    GameObject targetLook;
    public bool preventDeath;
    bool guard;

    private void Update()
    {
        Shader();
        CompleteDeath();
        if(!guard)
        {
            ui.CharacterUpdate(combat);
        }

        if(forceUI > Time.time)
        {
            ui.CharacterUpdate(true);
            ui.UpdateHealth(currentHealth, maxHealth);
        }
        
        if (player)
        {
            VisualCombatPath();
            CalculateAnimation();
            RotateCharacter();
            Travel();
            Attacking();

            if (interruptCooldown != -1)
            {
                if (Time.time > interruptCooldown)
                {
                    interruptCooldown = -1;
                }
            }

            if (!combat)
            {
                PlayerRegularRaycast();
            }
            else if (currentHealth > 0)
            {
                if (moveAllowed)             
                {

                    PlayerCombatRaycast();
                }
                else
                {
                    hitTile = null;
                }
            }
        }

        else if (timeActivate > Time.time)                    // ai behaviour
        {
            CalculateAnimation();
            RotateCharacter();
            Travel();
            Attacking();

            if (currentHealth <= 0)
            {
                combat = false;
            }

            if (!combat)
            {

            }
            else
            {
                if (moveAllowed)                  
                {
                    CharacterDecision();
                }
            }
        }
    }




    public void InitialiseCharacter(HexGrid targetHexGrid)
    {
        battleAreas = targetHexGrid.battleAreas;
        rb = GetComponent<Rigidbody>();
        worldObject = GetComponent<WorldObject>();
        hexGrid = targetHexGrid;
        pathFindingScript = targetHexGrid.findPath;

        if (player)
        {
            charRandomise.Initialise(0);
        }
        else if(enemy)
        {
            charRandomise.Initialise(2);
        }
        else
        {
            charRandomise.Initialise(1);
        }
        

        animator.SetFloat("ValX", animationState.x);
        animator.SetFloat("ValY", animationState.y);

        if (ui != null)
        {
            ui.Initialise(targetHexGrid, curve);

            ui.UpdateHealth(currentHealth, maxHealth);
        }

        if (player)
        {
            cam = targetHexGrid.playerCamera;

            deathAnimation = "BRB_infantry_10_death_A";
            attackAnimation = "BRB_infantry_07_attack_A";
            takeDamageAnimation = "BRB_infantry_09_take_damage";
        }
        else if (enemy)
        {
            deathAnimation = "UD_infantry_10_death_A";
            takeDamageAnimation = "UD_infantry_09_take_damage";
            attackAnimation = "UD_infantry_07_attack_A";

        }
        else      // npc
        {
            deathAnimation = "WK_spearman_10_death_A";
            takeDamageAnimation = "WK_spearman_09_take_damage";
            attackAnimation = "WK_spearman_07_attack";
        }

        if (interact)
        {
            interactable = GetComponent<Interactable>();
        }
    }

    public void UpdateGuard(bool active)
    {
        guard = true;
        if(active)
        {
            combat = true;
            currentHealth = 1;
          //  worldObject.recentTile.forceWalkable = false;
        }
        else
        {
        //    Debug.Log(Time.time);
            currentHealth = 0;
        //    worldObject.recentTile.forceWalkable = true;
        }
    }

    void VisualCombatPath()
    {
        if(hitTile != null)
        {
            if (!currentCombatPath.Contains(hitTile))
            {
                hitTile.pointerActivate = Time.time + 0.075f;
            }
        }

        for(int i = 0; i < currentCombatPath.Count; i++)
        {
            currentCombatPath[i].visualCombatActivate = Time.time + 0.075f;
        
        }

    }

    void CompleteDeath()
    {
        if(!preventDeath)
        {
            if (!deathCompleted && currentHealth <= 0)
            {
                deathCompleted = true;
                worldObject.recentTile.objectAtLocation = null;
                if (enemy)
                {
                    battleAreas.targetSkeletons.Remove(this);
                }
                else
                {
                    battleAreas.targetNPCs.Remove(this);
                }

            }
        }
    }


    void Shader()
    {
        charRandomise.UpdateShader();
        charRandomise.currentVisibility = currentVisibility;
        charRandomise.forceInvisibleTime = forceInvisibleTime;

        if (pointerTime > Time.time)
        {
            charRandomise.shaderTime = Time.time - 0.1f;
        }
        else
        {
            charRandomise.shaderTime = shaderTime;
        }
        
        charRandomise.selectedTime = selectedTime;


        if (currentHealth <= 0)
        {
            currentVisibility += Time.deltaTime * 0.3f;
            if(currentVisibility > 1)
            {
                currentVisibility = 1;
            }
        }
        else
        {
            currentVisibility -= Time.deltaTime * 0.3f;
            if (currentVisibility < 0)
            {
                currentVisibility = 0;
            }
        }


        worldObject.recentTile.selectTime = selectedTime;

    }


    public void Activate()
    {
        timeActivate = Time.time + 0.1f;

    }

    void Attacking()
    {
        if (attackTime != -1)
        {
            if (Time.time > attackTime + 1.3f)
            {
                attackTime = -1;
                midAction = false;
                moveComplete = true;

                attackTarget.ReceiveDamage(damage);
                
            }
        }
    }


    void PlayerRegularRaycast()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                HexTile hitTile = hit.transform.GetComponent<HexTile>();
                if (hit.transform.gameObject.CompareTag("Tile"))
                {
                    currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);
                    interactTarget = null;
                }
                else if (hit.transform.gameObject.CompareTag("Interact"))
                {
                    bool neighbourDetected = hitTile.DetectNeighbouringObject(gameObject);

                    if (neighbourDetected)
                    {
                        interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
                    }
                    else
                    {
                        currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
                        interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
                    }
                }
            }
        }
    }

    void PlayerCombatRaycast()
    {
        lastCombatRaycast = Time.time + 0.1f;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
        {
            GameObject raycastTile = hit.transform.gameObject;
            if (raycastTile != recentCombatTile)
            {
                recentCombatTile = raycastTile;

                neighbouring = false;
                attackOutOfDistance = false;
                hitTile = raycastTile.GetComponent<HexTile>();

                if (hit.transform.gameObject.CompareTag("Tile"))
                {
                    if(hitTile.objectAtLocation == null)
                    {
                        attackTarget = null;
                        npcTarget = null;
                        playerTarget = null;

                        if (!hitTile.tunnel)
                        {
                            hexGrid.TunnelsWalkable(false);
                        }

                        if (hitTile.walkable)
                        {
                            currentCombatPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);

                            while (currentCombatPath.Count > speed)
                            {
                                currentCombatPath.RemoveAt(currentCombatPath.Count - 1);
                            }
                        }

                        if (!hitTile.tunnel)
                        {
                            hexGrid.TunnelsWalkable(true);

                        }
                    }
                    else
                    {
                        hitTile = null;
                        currentCombatPath.Clear();
                    }

                }
                else if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    npcTarget = null;
                    playerTarget = null;
                    attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
                    if (hitTile.DetectNeighbouringObject(this.gameObject))
                    {
                        currentCombatPath.Clear();
                        neighbouring = true;
                    }
                    else
                    {
                        hexGrid.TunnelsWalkable(false);
                        currentCombatPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
                        hexGrid.TunnelsWalkable(false);

                        if (currentCombatPath.Count != 0)
                        {
                            while (currentCombatPath.Count > speed)
                            {
                                attackOutOfDistance = true;
                                currentCombatPath.RemoveAt(currentCombatPath.Count - 1);
                            }
                        }
                    }
                }
                else if (hit.transform.gameObject.CompareTag("NPC"))
                {
                    attackTarget = null;
                    playerTarget = null;
                    npcTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();

                    hexGrid.TunnelsWalkable(false);
                    currentCombatPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
                    hexGrid.TunnelsWalkable(false);

                    if (currentCombatPath.Count != 0)
                    {
                        while (currentCombatPath.Count > speed)
                        {
                            currentCombatPath.RemoveAt(currentCombatPath.Count - 1);
                        }
                    }
                }
                else if (hit.transform.gameObject.CompareTag("Player"))
                {
                    attackTarget = null;
                    npcTarget = null;
                    playerTarget = this;
                    currentCombatPath.Clear();
                }
                else
                {
                    hitTile = null;
                    currentCombatPath.Clear();
                    attackTarget = null;
                    npcTarget = null;
                    playerTarget = null;
                }


            }

            if (Input.GetMouseButtonDown(0) && hitTile != null)
            {
                currentPath = currentCombatPath;
                midAction = true;
                moveAllowed = false;
                currentTurns--; ;
                ui.UpdateTurns(currentTurns);   
                raycastTile = null;
                if (attackTarget != null)
                {
                    if (neighbouring)
                    {
                        attacking = true;

                    }
                    else
                    {
                        if (!attackOutOfDistance)
                        {
                            attacking = true;

                        }
                    }
                }
            }

        }
        else if (Physics.Raycast(ray, out hit, 100, environmentTargetMask))
        {
            GameObject raycastTile = hit.transform.gameObject;
            if (raycastTile != recentCombatTile)
            {
                recentCombatTile = raycastTile;
                currentCombatPath.Clear();
                attackTarget = null;
                npcTarget = null;
                playerTarget = null;
                hitTile = null;
            }
        }
        else
        {
            hitTile = null;
            currentCombatPath.Clear();
            attackTarget = null;
        }

        if(playerTarget != null)
        {
            playerTarget.forceInvisibleTime = Time.time + 0.1f;
        }
        else if(npcTarget != null)
        {
            npcTarget.selectedTime = Time.time + 0.1f;
        }
        else if(attackTarget != null)
        {
            attackTarget.selectedTime = Time.time + 0.1f;
        }


    }

    bool CharacterDetectNeighbours()
    {
        List<HexTile> neighbourTiles = hexGrid.GetNeighbours(worldObject.recentTile);
        List<CharacterController> neighbourCharacters = new List<CharacterController>();


        bool taskComplete = false;
        for (int i = 0; i < neighbourTiles.Count; i++)
        {
            if (neighbourTiles[i] != null)
            {
                if (neighbourTiles[i].objectAtLocation != null)
                {
                    if (neighbourTiles[i].objectAtLocation.CompareTag("Player") && enemy)
                    {
                        neighbourCharacters.Add(neighbourTiles[i].objectAtLocation.GetComponent<CharacterController>());
                    }
                    else if (neighbourTiles[i].objectAtLocation.CompareTag("NPC") && enemy)
                    {
                        neighbourCharacters.Add(neighbourTiles[i].objectAtLocation.GetComponent<CharacterController>());
                    }
                    else if (neighbourTiles[i].objectAtLocation.CompareTag("Enemy") && !enemy)
                    {
                        neighbourCharacters.Add(neighbourTiles[i].objectAtLocation.GetComponent<CharacterController>());
                    }
                }
            }

        }

        if (neighbourCharacters.Count > 0)
        {
            moveAllowed = false;
            taskComplete = true;
            midAction = true;
            attackTime = Time.time;
            attackTarget = neighbourCharacters[0];
            currentTurns--;
            ui.UpdateTurns(currentTurns);
        }

        return taskComplete;
    }

    void CharacterDecision()
    {
        if (!CharacterDetectNeighbours())
        {
            hexGrid.TunnelsWalkable(false);
            List<HexTile> targetTiles = new List<HexTile>();
            List<List<HexTile>> possiblePaths = new List<List<HexTile>>();

            for (int i = 0; i < nearbyControllers.Count; i++)
            {
                targetTiles.Add(nearbyControllers[i].worldObject.recentTile);
            }

            for (int i = 0; i < targetTiles.Count; i++)
            {
                possiblePaths.Add(pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, targetTiles[i]));
            }

            int shortestIndex = -1;
            int shortestListCount = 0;
            List<HexTile> calculatePath;
            for (int i = 0; i < possiblePaths.Count; i++)
            {
                calculatePath = possiblePaths[i];
                if (i == 0)
                {
                    shortestIndex = 0;
                    shortestListCount = calculatePath.Count;
                }
                else
                {
                    if (calculatePath.Count < shortestListCount)
                    {
                        shortestListCount = calculatePath.Count;
                        shortestIndex = i;
                    }
                }
            }

            if (shortestIndex != -1)
            {

                currentPath = possiblePaths[shortestIndex];
                attackTarget = nearbyControllers[shortestIndex];

                midAction = true;
                attacking = true;
                currentTurns--;
                ui.UpdateTurns(currentTurns); 
                moveAllowed = false;


            }
            else
            {

                currentTurns--;
                ui.UpdateTurns(currentTurns);
                moveAllowed = false;
            }

            if (currentPath.Count != 0)
            {
                while (currentPath.Count > speed)
                {
                    currentPath.RemoveAt(currentPath.Count - 1);
                    attacking = false;
                }
            }


            hexGrid.TunnelsWalkable(false);
        }
    }
        

 

    void CalculateAnimation()
    {
        animator.SetFloat("ValX", animationState.x);
        animator.SetFloat("ValY", animationState.y);

        if (combat)
        {
            if (moving)
            {
                animationState = Vector2.Lerp(animationState, new Vector2(1, -1), 4 * Time.deltaTime);
            }
            else
            {
                animationState = Vector2.Lerp(animationState, new Vector2(-1, -1), 4 * Time.deltaTime);
            }

            if (currentHealth <= 0)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    currentVisibility += Time.deltaTime;
                    animator.Play(deathAnimation);    

                }
            }
            else if (attackTime != -1)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    animator.Play(attackAnimation);  
                }
            }
            else if (receiveDamageTime != -1)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    animator.Play(takeDamageAnimation);   
                }

                if (Time.time > receiveDamageTime + 1)
                {
                    receiveDamageTime = -1;
                }
            }

        }
        else                    // idle
        {
            if (moving)
            {
                animationState = Vector2.Lerp(animationState, new Vector2(-1, 1), 4 * Time.deltaTime);
            }
            else
            {
                animationState = Vector2.Lerp(animationState, new Vector2(1, 1), 4 * Time.deltaTime);

            }
        }
    }

    void RotateCharacter()
    {

        if (currentPath.Count != 0)
        {
            Vector3 currentAngle = transform.localEulerAngles;
            lookDirection.LookAt(currentPath[0].targetLocation.transform);
            Vector3 targetRotation = lookDirection.transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(0, 0, 0),
               Mathf.LerpAngle(currentAngle.y, targetRotation.y, 5 * Time.deltaTime),
               Mathf.LerpAngle(0, 0, 0));

            transform.eulerAngles = currentAngle;
        }
        else if (attackTime != -1)
        {
            Vector3 currentAngle = transform.localEulerAngles;
            lookDirection.LookAt(attackTarget.transform);
            Vector3 targetRotation = lookDirection.transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(0, 0, 0),
               Mathf.LerpAngle(currentAngle.y, targetRotation.y, 5 * Time.deltaTime),
               Mathf.LerpAngle(0, 0, 0));

            transform.eulerAngles = currentAngle;
        }
        else if (interactTime != -1 && interactTarget != null)
        {
            Vector3 currentAngle = transform.localEulerAngles;
            lookDirection.LookAt(interactTarget.transform);
            Vector3 targetRotation = lookDirection.transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(0, 0, 0),
               Mathf.LerpAngle(currentAngle.y, targetRotation.y, 5 * Time.deltaTime),
               Mathf.LerpAngle(0, 0, 0));

            transform.eulerAngles = currentAngle;
        }
        else if (interactTime != -1 && characterInteract != null)
        {
            Vector3 currentAngle = transform.localEulerAngles;
            lookDirection.LookAt(characterInteract.transform);
            Vector3 targetRotation = lookDirection.transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(0, 0, 0),
               Mathf.LerpAngle(currentAngle.y, targetRotation.y, 5 * Time.deltaTime),
               Mathf.LerpAngle(0, 0, 0));

            transform.eulerAngles = currentAngle;
        }

        else if (lookTargetTime != -1)
        {
            if (Time.time > lookTargetTime + 1)
            {
                lookTargetTime = -1;
            }

            Vector3 currentAngle = transform.localEulerAngles;
            lookDirection.LookAt(targetLook.transform);
            Vector3 targetRotation = lookDirection.transform.eulerAngles;

            currentAngle = new Vector3(
               Mathf.LerpAngle(0, 0, 0),
               Mathf.LerpAngle(currentAngle.y, targetRotation.y, 5 * Time.deltaTime),
               Mathf.LerpAngle(0, 0, 0));

            transform.eulerAngles = currentAngle;

        }
    }

    public void ReceiveDamage(int damageVal)
    {
        currentHealth -= damageVal;
        receiveDamageTime = Time.time;

        ui.UpdateHealth(currentHealth, maxHealth);
    }
    public void ReceiveLookTarget(GameObject targetLookTarget)
    {
        lookTargetTime = Time.time;
        targetLook = targetLookTarget;
    }

    public void RestoreHealth()
    {
        //  currentHealth = maxHealth;
        //  healthUI.UpdateUI(currentHealth, maxHealth);
    }


    void Move(Vector3 targetLocation)
    {
        Vector3 targetVector = targetLocation - transform.position;
        targetVector = targetVector.normalized;
        targetVector *= 2.5f;
        rb.velocity = targetVector;
        moving = true;
    }


    void Travel()
    {
        if (currentPath.Count != 0)
        {
            Vector3 targetLocation = currentPath[0].targetLocation.transform.position;
            targetLocation.y = transform.position.y;

            bool verifyX = false;
            if (transform.position.x < targetLocation.x + 0.05f && transform.position.x > targetLocation.x - 0.05f)
            {
                verifyX = true;
            }

            bool verifyZ = false;
            if (transform.position.z < targetLocation.z + 0.05f && transform.position.z > targetLocation.z - 0.05f)
            {
                verifyZ = true;
            }

            if (verifyX && verifyZ)
            {
                worldObject.recentTile.RemoveObjectAtLocation();
                worldObject.recentTile = currentPath[0];
                worldObject.recentTile.PlaceObjectAtLocation(gameObject);

                currentPath.RemoveAt(0);
            }
        }

        if (currentPath.Count != 0)
        {
            Vector3 targetLocation = currentPath[0].targetLocation.transform.position;
            targetLocation.y = transform.position.y;
            Move(targetLocation);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            moving = false;

            if (attacking)
            {
                attacking = false;
                attackTime = Time.time;
                attackTarget.ReceiveLookTarget(this.gameObject);
            }
            else if (interactTarget != null && interactTime == -1)
            {
                interactTime = Time.time;
            }
            else
            {
                if (midAction && attackTime == -1)
                {
                    moveComplete = true;
                    midAction = false;
                }
            }

            if (interactTime != -1)
            {
                if (Time.time > interactTime + 0.5f)
                {
                    interactTime = -1;

                    interactTarget.Interact(this);
                    interactTarget = null;

                }
            }
        }
    }


    public void Interrupt()
    {
        rb.velocity = new Vector3(0, 0, 0);
        moving = false;
        currentPath.Clear();
        midAction = false;
    }


}

















/*
RaycastHit hit;
Ray ray = cam.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
{
    if (Input.GetMouseButtonDown(0))
    {
        HexTile hitTile = hit.transform.GetComponent<HexTile>();


        if (hit.transform.gameObject.CompareTag("Tile"))
        {
            if (!hitTile.tunnel)
            {
                hexGrid.TunnelsWalkable(false);
            }

            if (hitTile.walkable)
            {
                currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);

                if (currentPath.Count != 0)
                {
                    midAction = true;
                    currentUITurns--;
                }
            }

            if (!hitTile.tunnel)
            {
                hexGrid.TunnelsWalkable(true);

            }
        }
        else if (hit.transform.gameObject.CompareTag("Enemy"))
        {
            if (hitTile.DetectNeighbouringObject(this.gameObject))
            {
                midAction = true;
                attackTime = Time.time;
                attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
                attackTarget.ReceiveLookTarget(this);
                currentUITurns--;
            }
            else
            {
                hexGrid.TunnelsWalkable(false);
                currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
                hexGrid.TunnelsWalkable(false);

                if (currentPath.Count != 0)
                {
                    midAction = true;
                    attacking = true;
                    attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
                    currentUITurns--;

                }
            }
        }

    }
}
}
*/
/*
 *         attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
    if (hitTile.DetectNeighbouringObject(this.gameObject))
    {
        midAction = true;
        attackTime = Time.time;
    }
    else
    {
        hexGrid.TunnelsWalkable(false);
        currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
        hexGrid.TunnelsWalkable(false);

        if (currentPath.Count != 0)
        {
            midAction = true;
            attacking = true;
        }
    }*/
/*
 * 
 *
//   Debug.Log(combat + " " + Time.time + " " + currentTurns);
//  if(!combat || currentTurns > 0)
//   {
RaycastHit hit;
Ray ray = cam.ScreenPointToRay(Input.mousePosition);

if(combat)
{

}
else
{

}

if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
{
    if (Input.GetMouseButtonDown(0))
    {
        HexTile hitTile = hit.transform.GetComponent<HexTile>();
        if (hit.transform.gameObject.CompareTag("Tile"))
        {
            Debug.Log("hit " + Time.time);
            TileDecision(hitTile);
        }
        else if (hit.transform.gameObject.CompareTag("Enemy"))
        {
            EnemyTileDecision(hitTile);
        }
        else if (hit.transform.gameObject.CompareTag("Interact"))
        {
            Debug.Log("yolo: " + Time.time);
            InteractTileDecision(hitTile);
        }
    }
}
}
*/

/*
    void TileDecision(HexTile hitTile)
    {
        currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);
        interactTarget = null;
        midAction = true;
    }

    void EnemyTileDecision(HexTile hitTile)
    {
        attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
        if (hitTile.DetectNeighbouringObject(this.gameObject))
        {
            midAction = true;
            attackTime = Time.time;
        }
        else
        {
            hexGrid.TunnelsWalkable(false);
            currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
            hexGrid.TunnelsWalkable(false);

            if (currentPath.Count != 0)
            {
                midAction = true;
                attacking = true;
            }
        }
    }



void InteractTileDecision(HexTile hitTile)
{
Debug.Log(Time.time);
bool neighbourDetected = hitTile.DetectNeighbouringObject(gameObject);
if (neighbourDetected)
{
//   Debug.Log(Time.time + "neighbourDetected");
interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
}
else
{
currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
}
}
*/


/*
if (Physics.Raycast(ray, out hit, 100, interactTargetMask))
{
    Debug.Log(Time.time);
    if (Input.GetMouseButtonDown(0))
    {
        Debug.Log(Time.time);
        HexTile hitTile = hit.transform.GetComponent<HexTile>();                                // interact
        bool neighbourDetected = hitTile.DetectNeighbouringObject(gameObject);      
        if(neighbourDetected)
        {
         //   Debug.Log(Time.time + "neighbourDetected");
            interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
        }
        else
        {
            currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
            interactTarget = hitTile.objectAtLocation.GetComponent<Interactable>();
         //   Debug.Log(Time.time + "else");
        }   //interactTarget
    }
}
else if (Physics.Raycast(ray, out hit, 100, tileTargetMask))             // get to interact
{
    if (Input.GetMouseButtonDown(0))
    {
        HexTile hitTile = hit.transform.GetComponent<HexTile>();
        currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);
        //   Debug.Log(Time.time + "if (Physics.Raycast(ray, out hit, 100, tileTargetMask))");
        interactTarget = null;
    }
}
}
}

*/

// Debug.Log(Time.time);
/*

 if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
 {
     if (Input.GetMouseButtonDown(0))
     {

         Debug.Log(hitTile.gameObject.name +" : " + Time.time);

         Debug.Log(currentPath.Count);
     }
 }
}
}
*/

/*
if (combat)
{
    RaycastHit hit;
    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    bool taskComplete = false;
    if (Physics.Raycast(ray, out hit, 100, enemyTargetMask))
    {
        if (Input.GetMouseButtonDown(0))
        {
            HexTile hitTile = hit.transform.GetComponent<HexTile>();                                // combat
            attackTarget = hitTile.objectAtLocation.GetComponent<CharacterController>();
            if (hitTile.DetectNeighbouringObject(this.gameObject))
            {
                taskComplete = true;
                midAction = true;
                attackTime = Time.time;
            }
            else
            {
                hexGrid.TunnelsWalkable(false);
                currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
                hexGrid.TunnelsWalkable(false);

                if (currentPath.Count != 0)
                {
                    //    attackTime = -1;
                    taskComplete = true;
                    midAction = true;
                    attacking = true;
                }
            }
        }
    }

    if (!taskComplete)
    {
        if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                HexTile hitTile = hit.transform.GetComponent<HexTile>();
                if (!hitTile.tunnel)
                {
                    hexGrid.TunnelsWalkable(false);
                }

                if (hitTile.walkable)
                {
                    currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);

                    if (currentPath.Count != 0)
                    {
                        midAction = true;
                    }
                    else
                    {
                        //   Debug.Log("path == 0");
                    }
                }

                if (!hitTile.tunnel)
                {
                    hexGrid.TunnelsWalkable(true);
                }
            }
        }
    }
}
}

*/



//  void CharacterDecision()
//  {
//     Debug.Log(gameObject.name + Time.time);
//    currentTurns--;
//  }

















/*
 *     void UpdatePath()
    {
        //    for (int i = 0; i < visualPath.Count; i++)
        //  {
        //    visualPath[i].PointAtTile();
        //  }
    }
 *         int shortestIndex = -1;
int shortestListCount = 0;
List<HexTile> currentPath;
for (int i = 0; i < possiblePaths.Count; i++)
{
    currentPath = possiblePaths[i];
    if (i == 0)
    {
        shortestIndex = 0;
        shortestListCount = currentPath.Count;
    }
    else
    {
        if(currentPath.Count < shortestListCount)
        {
            shortestListCount = currentPath.Count;
            shortestIndex = i;
        }
    }
}

if(shortestIndex != -1)
{
    path = possiblePaths[shortestIndex];
}




 *             if (!taskComplete)
    {
        if (Physics.Raycast(ray, out hit, 100, tileTargetMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                HexTile hitTile = hit.transform.GetComponent<HexTile>();
                if (!hitTile.tunnel)
                {
                    hexGrid.TunnelsWalkable(false);
                }

                if (hitTile.walkable)
                {
                    currentPath = pathFindingScript.RequestPath(worldObject.recentTile, hitTile);

                    if (currentPath.Count != 0)
                    {
                        midAction = true;
                    }
                    else
                    {
                        //   Debug.Log("path == 0");
                    }
                }

                if (!hitTile.tunnel)
                {
                    hexGrid.TunnelsWalkable(true);
                }
            }
        }
    }
*/

//    List<List<HexTile>> possiblePaths = new List<List<HexTile>>();

//   for (int i = 0; i < neighbourCharacters.Count; i++)
//  {
//     List<HexTile> path = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, neighbourCharacters[i].worldObject.recentTile);
//  }

/*
bool taskComplete = false;

RaycastHit hit;
Ray ray = cam.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out hit, 100, enemyTargetLayer))
{
if (Input.GetMouseButtonDown(0))
{
HexTile hitTile = hit.transform.GetComponent<HexTile>();
//   attackSkeleton = hitTile.objectAtLocation.GetComponent<Skeleton>();
if (hitTile.DetectNeighbouringObject(this.gameObject))
{
 //   attack = true;
    taskComplete = true;
}
else
{
    hexGrid.TunnelsWalkable(false);
    currentPath = pathFindingScript.RequestShortestPathToInteractable(worldObject.recentTile, hitTile);
    hexGrid.TunnelsWalkable(false);

    if (currentPath.Count != 0)
    {
      //  attack = true;
        taskComplete = true;
    }
}
}
}

if (!taskComplete)
{
if (Physics.Raycast(ray, out hit, 100, tileTargetLayer))
{
if (Input.GetMouseButtonDown(0))
{
    HexTile hitTile = hit.transform.GetComponent<HexTile>();
    if (!hitTile.tunnel)
    {
        hexGrid.TunnelsWalkable(false);
    }

    if (hitTile.walkable)
    {
//         start = worldObject.recentTile;
  //      end = hitTile;
        currentPath = pathFindingScript.RequestPath(start, end);

        if (currentPath.Count != 0)
        {
  //          playerMove = true;
        }
        else
        {
            //   Debug.Log("path == 0");
        }
    }

    if (!hitTile.tunnel)
    {
        hexGrid.TunnelsWalkable(true);
    }
}
}
}

*/










/*

               }
           }

       }
   }
                   */











/*
if (!attack && turnsLeft > 0)
{
    if (playerMove)
    {
        turnsLeft--;
        playerMove = false;
        if (turnsLeft == 0)
        {
            //  battleAreas.SubmitTurn();
        }
    }
}
}
}
*/


/*

}

*/
/*
public void InitialisePlayer(Camera mainCam, BattleAreas battleAreas, FindPath findPath)                    //(FindPath pathScript, CameraController camControl, FindPath pathScript)
{
    pathFindingScript = findPath;
    cam = mainCam;
    battleAreas.player = this;
}
*/

/*
if(fallAgain && Time.time > 0.1f)
{
   // GetComponent<ObjectsFall>().EnableDuringGame();
}
RotateCharacter();
CalculateAnimation();
if (!combat)
{
    RaycastEstablishPath();
}
else if (!moving && !attack && turnsLeft != 0)
{
    RaycastBattleChoice();
}

Travel();

if (combat)
{
    Attack();
}

}
*/


//  void CalculateAnimation()
// {
/*
animator.SetFloat("ValX", animationState.x);
animator.SetFloat("ValY", animationState.y);
if (combat)
{
    if (moving)
    {
        animationState = Vector2.Lerp(animationState, new Vector2(1, -1), 2 * Time.deltaTime);
    }
    else
    {
        animationState = Vector2.Lerp(animationState, new Vector2(-1, -1), 2 * Time.deltaTime);
    }

    if(currentHealth <= 0)
    {
        int random = Random.Range(0, 2);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Debug.Log(Time.time);
            if (random == 0)
            {
                animator.Play("BRB_infantry_10_death_B");
            }
            else
            {
                animator.Play("BRB_infantry_10_death_A");
            }
        }
    }
    else if(attackTime != -1)
    {
        int random = Random.Range(0, 2);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Debug.Log(Time.time);
            if (random == 0)
            {
                animator.Play("BRB_infantry_07_attack_A");
            }
            else
            {
                animator.Play("BRB_infantry_08_attack_B");
            }
        }
    }
    else if (receiveDamageTime != -1)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            animator.Play("BRB_infantry_09_take_damage");
        }

        if(Time.time > receiveDamageTime +1)
        {
            receiveDamageTime = -1;
        }
    }

}
else                    // idle
{
    if (moving)
    {
        animationState = Vector2.Lerp(animationState, new Vector2(-1, 1), 2 * Time.deltaTime);
    }
    else
    {
        animationState = Vector2.Lerp(animationState, new Vector2(1, 1), 2 * Time.deltaTime);
    }
}
*/




//  void Attack()
//{

//}
/*
if(attack && currentPath.Count == 0)
{
    if(attackTime == -1)
    {
        attackTime = Time.time;
    }
    else if(Time.time > attackTime + 5)
    {
        Debug.Log(Time.time);
        attack = false;
        attackTime = -1;
        turnsLeft--;
        attackSkeleton.ReceiveDamage(currentDamage);


        if (turnsLeft == 0)
        {
          //  Debug.Log(turnsLeft);
      //      battleAreas.SubmitTurn();
        }
    }
}
*/

















/*
 * 
 * 
     // currentAnimationState = Vector2.Lerp(currentAnimationState, attacking, 2 * Time.deltaTime);
     //   Debug.Log(currentAnimationState);
     /*
     if(combat)
     {
         if(receiveDamageTime != -1)
         {
             currentAnimationState = Vector2.Lerp(currentAnimationState, takeDamage, 10 * Time.deltaTime);
             if(Time.time > receiveDamageTime + 2)
             {
                 receiveDamageTime = Time.time;
             }

         }
         else if(attackTime != -1)
         {
             Debug.Log(Time.time + " : " + currentAnimationState);
             currentAnimationState = Vector2.Lerp(currentAnimationState, attacking, 10 * Time.deltaTime);
         }
         else if(moving)
         {
             currentAnimationState = Vector2.Lerp(currentAnimationState, charge, 10 * Time.deltaTime);
         }
         else
         {
             currentAnimationState = Vector2.Lerp(combatIdle, charge, 10 * Time.deltaTime);
         }
     }
     else if(moving)
     {
         currentAnimationState = Vector2.Lerp(currentAnimationState, run, 10 * Time.deltaTime);
     }
     else            // regular idle
     {
         currentAnimationState = Vector2.Lerp(currentAnimationState, idle, 10 * Time.deltaTime);
     }

 //  animator.SetFloat("ValX", currentAnimationState.x);
 //  animator.SetFloat("ValY", currentAnimationState.y);



     else if (moveEnabled)
     {
         RaycastHit hit;
         Ray ray = cam.ScreenPointToRay(Input.mousePosition);
         if (Physics.Raycast(ray, out hit, 100, enemyTargetLayer))
         {
             if (Input.GetMouseButtonDown(0))
             {
                 HexTile hitTile = hit.transform.GetComponent<HexTile>();
                 bool moveRequired = true;
                 if(recentTileChoice == null)
                 {
                     recentTileChoice = hitTile;
                 }
                 else if(recentTileChoice == hitTile)
                 {
                     moveRequired = false;
                 }

                 if(moveRequired)
                 {
                     start = worldObject.recentTile;
                     List<HexTile> availableTiles = hitTile.RequestNeighbours(true, false);
                     List<List<HexTile>> possiblePaths = new List<List<HexTile>>();
                     List<HexTile> shortestPath = new List<HexTile>();
                     for (int i = 0; i < availableTiles.Count; i++)
                     {
                         currentPath = pathFindingScript.RequestPath(start, availableTiles[i]);
                         possiblePaths.Add(currentPath);
                     }

                     if (availableTiles.Count != 0)
                     {
                         for (int i = 0; i < possiblePaths.Count; i++)
                         {
                             if (i == 0)
                             {
                                 shortestPath = possiblePaths[i];
                             }
                             else if (possiblePaths.Count < shortestPath.Count)
                             {
                                 shortestPath = possiblePaths[i];
                             }

                             attack = true;
                             moveEnabled = false;
                         }
                     }
                 }
                 else
                 {
                     attack = true;
                     moveEnabled = false;
                 }
             }
         }
     }
 }
 */


/*
List<HexTile> currentPath = possiblePaths[i];
int currentPathDistance = currentPath.Count;
if (i == 0)
{
pathDistance = currentPathDistance;
}
else
{
if (currentPathDistance < pathDistance)
{
pathDistance = currentPathDistance;
//     index = i;
}
}
}
*/


//        currentPath = possiblePaths[index];

//  void RegularBehaviourUpdate()
//  {

//   }

/*
if (moving && currentPath.Count != 0)
{
    objectLookTransform.position = currentPath[0].targetLocation.transform.position;

    objectRotateTransform.LookAt(objectLookTransform);

    currentRotation = Vector3.Lerp(currentRotation, objectRotateTransform.eulerAngles, 10 * Time.deltaTime);
    currentRotation.x = 0;
    transform.eulerAngles = currentRotation;
}

if (!moving)
{
    VisualRaycastTiles();
}

if (visualTilesEnabled && !combat)
{
    UpdatePath();
}
*/

//  RaycastEstablishPath(false);




//   public void LocationReached(bool state)
//        locationReached = state;
//  }



/*
GameObject raycastObject = hit.transform.gameObject;
if(raycastObject != currentRaycast)
{
    currentRaycast = raycastObject;
    raycastTimeStart = Time.time;
    hexTilesEnabled = false;
}
else if(Time.time > raycastTimeStart + 1)
{
    hexTilesEnabled = true;
}

if(Input.GetMouseButtonDown(0))
{
    start = transform.parent.GetComponent<HexTile>();
    end = raycastObject.GetComponent<HexTile>();
    currentPath = pathFindingScript.RequestPath(start, end);
}
}
else
{
hexTilesEnabled = false;
}
}
*/






/*
if (Physics.Raycast(ray, out hit, 100, tileTargetLayer))
{
    if (hit.transform.gameObject != currentRaycastObject)
    {
        currentRaycastObject = hit.transform.gameObject;
        raycastFirstTime = Time.time;
    }
    else if (Time.time > raycastFirstTime + 0.5f)
    {
        ExecuteVisualPath();
    }
}
}

void ExecuteVisualPath()
{

}
}
*/
/*
GameObject tempRaycastObject = hit.transform.gameObject;
if (target != recentRaycast)
{
    recentRaycast = target;
    timeFirstRaycast = Time.time;
}
else
{
    if (Time.time > timeFirstRaycast + 0.25f)
    {
        recentHexControl.GetRaycast();
    }
}
}
}


}
*/
/*


void ExecuteMovement()
{

}

void CalculatePath()
{

}


}

GameObject target = hit.transform.gameObject;
if (target != recentRaycast)
{
    recentRaycast = target;
    timeFirstRaycast = Time.time;
}
else
{
    if (Time.time > timeFirstRaycast + 0.25f)
    {
        recentHexControl.GetRaycast();
    }
}
}
else
{
recentRaycast = null;
}
}
}
*/
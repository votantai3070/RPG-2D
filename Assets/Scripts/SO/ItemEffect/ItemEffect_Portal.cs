using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Item effect data - Portal Scroll", menuName = "RPG Setup/Item Data/Item Effect/Portal Scroll")]
public class ItemEffect_Portal : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("Cannot use portal in the town.");
            return;
        }

        Player player = Player.instance;


        Vector3 portalPosition = player.transform.position + new Vector3(player.faceDir * 1.5f, 0);

        Object_Portal.instance.ActivatePortal(portalPosition, player.faceDir);
    }
}

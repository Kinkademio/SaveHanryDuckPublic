using UnityEngine;
using UnityEngine.UI;

public class TableFill : MonoBehaviour
{
    [SerializeField] GameObject Row;
    [SerializeField] GameObject Cell;

    [SerializeField] GameObject Table;
    [SerializeField] GameObject TableBody;
    [SerializeField] GameObject TableHeaderRow;
    [SerializeField] Text RecordsName;
     

    public void clearTable()
    {
        //������� �������
        destroyAllChildrenObjects(TableBody);
        //������� �����
        destroyAllChildrenObjects(TableHeaderRow);

    }

    private void destroyAllChildrenObjects(GameObject parent)
    {
        int childCounter = parent.transform.childCount;
        for(int i =0; i<childCounter; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    public void fillTable()
    {
        clearTable();
        //�������� ������ ���� ��������
        SavedScore[] scores = ScoreController.getAllSavedScosres();
        if (scores.Length == 0)
        {
            Table.SetActive(false);
            RecordsName.text = "� ��� ���� ��� ��������";
        }
        else
        {
            Table.SetActive(true);
            RecordsName.text = "�������";
        }
        bool headerCreate = false;
        //�������� ������������ ����
        createHeaderCell("�");
        createHeaderCell("����:");
        //�������� ������� ��������
        int rowIndex = 1;
        foreach (SavedScore score in scores)
        {
            GameObject currentRow = Instantiate(Row, TableBody.transform);
            //�������� ������ � �������� � ����� � ������ ������
            createBodyCell(rowIndex.ToString(), currentRow.transform);
            createBodyCell(score.saveDate, currentRow.transform);

            foreach (Score oneScore in score.savedScore)
            {
                //�������� ������ �������
                if (!headerCreate)
                {
                    createHeaderCell(oneScore.scoreNameForMenu);
                }
                //�������� ���� �������
                createBodyCell(oneScore.scoreValue, currentRow.transform);
            }
            if (!headerCreate) headerCreate = true;
            rowIndex++;
        }
    }

    private Text checkCell(GameObject cell)
    {
        Text cellText = cell.GetComponentInChildren<Text>();
        if (!cellText) throw new System.Exception("� ������ ������ ������� " + cell + "����������� ��������� Text!");
        return cellText;
    }

    private void createHeaderCell(string value)
    {
        GameObject headerCell = Instantiate(Cell, TableHeaderRow.transform);
        Text cellText = checkCell(headerCell);
        cellText.text = value;
    }

    private void createBodyCell(string value, Transform currentRow)
    {
        GameObject bodyCell = Instantiate (Cell, currentRow);
        Text bodyCellText = checkCell(bodyCell);
        bodyCellText.text = value;

    }

}

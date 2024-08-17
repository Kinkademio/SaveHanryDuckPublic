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
        //Очищаем таблицу
        destroyAllChildrenObjects(TableBody);
        //Очищаем хедер
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
        //Получаем список всех рекордов
        SavedScore[] scores = ScoreController.getAllSavedScosres();
        if (scores.Length == 0)
        {
            Table.SetActive(false);
            RecordsName.text = "У вас пока нет рекордов";
        }
        else
        {
            Table.SetActive(true);
            RecordsName.text = "Рекорды";
        }
        bool headerCreate = false;
        //Создадим обязательные поля
        createHeaderCell("№");
        createHeaderCell("Дата:");
        //Заполним таблицу Рекордов
        int rowIndex = 1;
        foreach (SavedScore score in scores)
        {
            GameObject currentRow = Instantiate(Row, TableBody.transform);
            //Создадим ячейку с индексом и датой в начале строки
            createBodyCell(rowIndex.ToString(), currentRow.transform);
            createBodyCell(score.saveDate, currentRow.transform);

            foreach (Score oneScore in score.savedScore)
            {
                //Заполним голову таблицы
                if (!headerCreate)
                {
                    createHeaderCell(oneScore.scoreNameForMenu);
                }
                //Заполним тело таблицы
                createBodyCell(oneScore.scoreValue, currentRow.transform);
            }
            if (!headerCreate) headerCreate = true;
            rowIndex++;
        }
    }

    private Text checkCell(GameObject cell)
    {
        Text cellText = cell.GetComponentInChildren<Text>();
        if (!cellText) throw new System.Exception("В объкте ячейки таблицы " + cell + "отсутствует компонент Text!");
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

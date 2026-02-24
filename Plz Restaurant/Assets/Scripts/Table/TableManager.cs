using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    // 테이블을 담을 배열 - 유니티 엔진 상에서 넣는 방식으로 구현.
    // ** 중요 ** 테이블 번호 순서에 맞게 넣어야 함
    [SerializeField]
    private Table[] tables = null;
    public IEnumerable<Table> Tables => tables; // 외부에서 순회를 위해 접근

    public int tableNum { get { return tables.Length; } }


    // 지금 단계에선 더 필요한 게 없어보임.
    // 이후 필요하면 추가할 예정

    /// <summary>
    /// Tables 배열의 getter
    /// </summary>
    /// <param name="tableNum">
    /// ** 0 ~ n - 1 **
    /// </param>
    /// <returns>
    /// Tables 객체
    /// </returns>
    public Table GetTable(int tableNum)
    {
        if (tables == null || tables.Length <= tableNum) return null;
        return tables[tableNum]; // 테이블 번호는 1번부터, 인덱스는 0번부터.
    }

    // 테이블 매니저 끄고 키는 함수
    public void EnableTableManager()
    {
        gameObject.SetActive(true);
    }

    public void DisableTableManager()
    {
        gameObject.SetActive(false);
    }

    public void AllTalbleClear()
    {
        foreach (Table table in tables)
        {
            table.DestroyMoney(); // 게임 종료 상태에서 테이블 위에 남아 있는 돈 삭제
            table.VisitorDeparture(); // 테이블에 남은 손님 퇴장
            table.TableCleanUp(); // 변수 초기화, 음식 오브젝트 삭제, 손님 정보 삭제
        }
    }
}

#include "StoryJson.h"
#include <string>
using std::string;

namespace HeavenGateEditor {

int HeavenGateEditor::StoryJson::AddWord(StoryWord * const word)
{
    m_nodes.push_back(word);
    return static_cast<int>( m_nodes.size());
}

int HeavenGateEditor::StoryJson::AddNode(StoryNode* const node){
    m_nodes.push_back(node);
      return static_cast<int>( m_nodes.size());
}
int HeavenGateEditor::StoryJson::AddWord(string name, string content)
{
    return  AddWord(name.c_str(), content.c_str());
}

int  HeavenGateEditor::StoryJson::AddWord(const char* name, const char* content){
    StoryWord* word = new StoryWord();
    strcpy(word->m_name, name);
    strcpy(word->m_content, content);

    return  AddWord(word);
}

const HeavenGateEditor::StoryNode * const HeavenGateEditor::StoryJson::GetNode(int index) const
{
    return m_nodes[index];
}

HeavenGateEditor::StoryNode * const HeavenGateEditor::StoryJson::GetNode(int index)
{
    return const_cast<StoryNode*>(
        static_cast<const StoryJson&>(*this).GetNode(index));
}

int HeavenGateEditor::StoryJson::Size() const
{
    return  static_cast<int>( m_nodes.size() );
}

void StoryJson::SetFullPath(const char* fullPath){
    strcpy(m_fullPath, fullPath);
}
const char* StoryJson::GetFullPath(){
    return m_fullPath;
}

bool StoryJson::IsExistFullPath()const{
    return m_fullPath != nullptr;
}


void to_json(json & j, const StoryWord & p)
{
    j = json{
        {"nodeType", p.m_nodeType},
        {"name", p.m_name},
        { "content", p.m_content }
    };
   
}
void to_json(json& j, const StoryLabel& p)
{
    j = json{
        {"nodeType", p.m_nodeType},
        {"label", p.m_labelId}

    };
}

void to_json(json& j, const StoryJump& p)
{
    j = json{
        {"nodeType", p.m_nodeType},
        {"jump",p.m_jumpId}

    };
}
void to_json(json & j, const StoryJson & story)
{
    for (int i = 0; i < story.Size(); i++)
    {
        const StoryNode* const pNode = story.GetNode(i);
     
        if (pNode->m_nodeType == NodeType::word)
        {
            const StoryWord*const pWord = static_cast<const StoryWord*const>(pNode);
            j.push_back(*pWord);
        }

           if (pNode->m_nodeType == NodeType::label)
           {
               const StoryLabel*const pLabel = static_cast<const StoryLabel*const>(pNode);
               j.push_back(*pLabel);
           }
        if (pNode->m_nodeType == NodeType::jump)
              {
                  const StoryJump*const pJump = static_cast<const StoryJump*const>(pNode);
                  j.push_back(*pJump);
              }
    }
}


/* String version, Archieved */

//void from_json(const json & j, StoryWord & p)
//{
//    j.at("name").get_to(p.m_name);
//    j.at("content").get_to(p.m_content);
//}

void from_json(const json & j, StoryWord & p)
{
    strcpy( p.m_name, j.at("name").get_ptr<const json::string_t *>()->c_str() );
    strcpy( p.m_content, j.at("content").get_ptr<const json::string_t *>()->c_str() );
}

void from_json(const json & j, StoryJson & p)
{
    for (int i = 0 ; i < j.size(); i++)
    {
        char enumString[ MAX_ENUM_LENGTH ];
        strcpy(enumString, j[i].at("nodeType").get_ptr<const json::string_t *>()->c_str());
        if(strcmp(enumString, "label") == 0){
            StoryLabel* node = new StoryLabel;
            node->m_nodeType = NodeType::label;
            *node = j[i];
            p.AddNode(node);
        }

        
        StoryWord* word = new StoryWord();
        *word = j[i];
        p.AddWord(word);

    }
    // iterate the array
   /* for (json::iterator it = j.begin(); it != j.end(); ++it) {
        StoryWord* word = new StoryWord();
        *word = *it;
        p.AddWord(word);
    }*/
}

void from_json(const json& j, StoryLabel& p){
    strcpy( p.m_labelId, j.at("label").get_ptr<const json::string_t *>()->c_str() );

}
void from_json(const json& j, StoryJump& p){
    strcpy( p.m_jumpId, j.at("jump").get_ptr<const json::string_t *>()->c_str() );
}

}
